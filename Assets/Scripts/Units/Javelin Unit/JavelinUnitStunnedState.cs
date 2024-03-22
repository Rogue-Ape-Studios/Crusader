using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnitStunnedState : IJavelinUnitState
    {
        private float _timer;
        public void EnterState(JavelinUnit javelinUnit)
        {
            javelinUnit.LocalAnimator.SetTrigger("Stunned");
        }

        public JavelinUnitStateId GetId()
        {
            return JavelinUnitStateId.Stunned;
        }

        public void UpdateState(JavelinUnit javelinUnit)
        {
            if (Timer(javelinUnit.SelfStunDuration))
            {
                javelinUnit.ChangeState(JavelinUnitStateId.Chase);
            }
        }

        private bool Timer(float time)
        {
            _timer += Time.deltaTime;
            if (_timer >= time)
            {
                _timer = 0;
                return true;
            }
            else { return false; }
        }
    }
}
