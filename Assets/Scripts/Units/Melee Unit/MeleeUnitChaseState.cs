using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class MeleeUnitChaseState : IMeleeUnitState
    {
        public void EnterState(MeleeUnit meleeUnit)
        {

        }

        public void ExitState(MeleeUnit meleeUnit)
        {

        }

        public void UpdateState(MeleeUnit meleeUnit)
        {
            meleeUnit.LocalUnitMovement.MoveToPlayer();
            Vector3 vectordistance = PlayerTracker.instance.playerTransform.position - meleeUnit.transform.position;
            if (vectordistance.magnitude < meleeUnit.StartAttackDistance)
            {
                meleeUnit.ChangeState(meleeUnit.AttackState);
            }
        }
    }
}
