using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnitDeathState : IJavelinUnitState
    {
        private float _timer = 0f;
        public void EnterState(JavelinUnit javelinUnit)
        {
            javelinUnit.LocalAnimator.SetTrigger("Death");
            javelinUnit.TurnOffCollider();
        }

        public JavelinUnitStateId GetId()
        {
            return JavelinUnitStateId.Death;
        }

        public void UpdateState(JavelinUnit javelinUnit)
        {
            if (Timer(javelinUnit.DestroyTime))
            {
                javelinUnit.DestroySelf();
            }
        }

        private bool Timer(float time)
        {
            _timer += Time.deltaTime;
            if (_timer >= time)
            {
                return true;
            }
            else { return false; }
        }
    }
}