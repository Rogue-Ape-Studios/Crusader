using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public class AxeUnitDeathState : IAxeUnitState
    {
        private float _timer = 0f;
        public void EnterState(AxeUnit axeUnit)
        {
            axeUnit.LocalAnimator.SetTrigger("Death");
            axeUnit.TurnOffCollider();
        }

        public AxeUnitStateId GetId()
        {
            return AxeUnitStateId.Death;
        }

        public void UpdateState(AxeUnit axeUnit)
        {
            if (Timer(axeUnit.DestroyTime))
            {
                axeUnit.DestroySelf();
            }
        }
        private bool Timer(float time)
        {
            _timer += Time.deltaTime;
            if(_timer >= time)
            {
                return true;
            }
            else { return false; }
            
        }

    }
}
