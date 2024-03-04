using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class MeleeUnitAttackState : IMeleeUnitState
    {
        public void EnterState(MeleeUnit meleeUnit)
        {
            meleeUnit.LocalAnimator.SetTrigger("Attack");
        }

        public void ExitState(MeleeUnit meleeUnit)
        {
            
        }

        public void UpdateState(MeleeUnit meleeUnit)
        {
            meleeUnit.ChangeState(meleeUnit.ChaseState);
        }
    }
}
