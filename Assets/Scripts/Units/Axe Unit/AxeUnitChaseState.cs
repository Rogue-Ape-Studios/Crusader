using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public class AxeUnitChaseState : IAxeUnitState
    {
        public void EnterState(AxeUnit axeUnit)
        {
            axeUnit.LocalAnimator.SetTrigger("Chase");
        }

        public AxeUnitStateId GetId()
        {
            return AxeUnitStateId.Chase;
        }

        public void UpdateState(AxeUnit axeUnit)
        {
            axeUnit.LocalUnitMovement.MoveToPlayer();
            if (axeUnit.PlayerDistance() < axeUnit.StartAttackDistance
                && axeUnit.LocalAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                axeUnit.ChangeState(AxeUnitStateId.Attack);
            }
        }
    }
}
