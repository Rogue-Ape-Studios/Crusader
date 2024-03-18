using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public class AxeUnitAttackState : IAxeUnitState
    {
        public void EnterState(AxeUnit axeUnit)
        {
            axeUnit.LocalAnimator.SetTrigger("Attack");
        }

        public AxeUnitStateId GetId()
        {
            return AxeUnitStateId.Attack;
        }

        public void UpdateState(AxeUnit axeUnit)
        {
            axeUnit.LocalUnitMovement.FacePlayer();
            if (axeUnit.PlayerDistance() > axeUnit.StopAttackDistance 
                && axeUnit.LocalAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                axeUnit.ChangeState(AxeUnitStateId.Chase);
            }
        }
    }
}
