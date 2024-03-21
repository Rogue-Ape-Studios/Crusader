using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public class AxeUnitStunnedState : IAxeUnitState
    {
        private float _timer;
        public void EnterState(AxeUnit axeUnit)
        {
            axeUnit.LocalAnimator.SetTrigger("Stunned");
        }

        public AxeUnitStateId GetId()
        {
            return AxeUnitStateId.Stunned;
        }

        public void UpdateState(AxeUnit axeUnit)
        {
            if (Timer(axeUnit.SelfStunDuration))
            {
                axeUnit.ChangeState(AxeUnitStateId.Chase);
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