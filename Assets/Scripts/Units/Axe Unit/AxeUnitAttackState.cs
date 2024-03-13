using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudios.Crusader.Units.AxeUnit
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
            axeUnit.ChangeState(AxeUnitStateId.Chase);
        }
    }
}
