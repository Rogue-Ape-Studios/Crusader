using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class AxeUnitAttackState : IAxeUnitState
    {
        public void EnterState(AxeUnit meleeUnit)
        {
            meleeUnit.LocalAnimator.SetTrigger("Attack");
        }

        public void UpdateState(AxeUnit meleeUnit)
        {
            meleeUnit.ChangeState(meleeUnit.ChaseState);
        }
    }
}
