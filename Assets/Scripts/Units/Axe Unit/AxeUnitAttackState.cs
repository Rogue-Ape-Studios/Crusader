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
            if (!axeUnit.IsInRange())
            {
                axeUnit.ChangeState(AxeUnitStateId.Chase);
            }
        }
    }
}
