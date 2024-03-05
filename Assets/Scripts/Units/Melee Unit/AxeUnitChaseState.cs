using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class AxeUnitChaseState : IAxeUnitState
    {
        public void EnterState(AxeUnit AxeUnit)
        {
            //start run animation here when available
        }

        public void UpdateState(AxeUnit AxeUnit)
        {
            AxeUnit.LocalUnitMovement.MoveToPlayer();
            Vector3 vectordistance = AxeUnit.LocalUnitMovement._playerTransform.position - AxeUnit.transform.position;
            if (vectordistance.magnitude < AxeUnit.StartAttackDistance)
            {
                AxeUnit.ChangeState(AxeUnit.AttackState);
            }
        }
    }
}
