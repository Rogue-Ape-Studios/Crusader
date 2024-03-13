using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudios.Crusader.Units.AxeUnit
{
    public class AxeUnitChaseState : IAxeUnitState
    {
        public void EnterState(AxeUnit axeUnit)
        {
            //start run animation here when available
        }

        public AxeUnitStateId GetId()
        {
            return AxeUnitStateId.Chase;
        }

        public void UpdateState(AxeUnit axeUnit)
        {
            axeUnit.LocalUnitMovement.MoveToPlayer();
            Vector3 vectordistance = axeUnit.LocalUnitMovement._playerTransform.position - axeUnit.transform.position;
            if (vectordistance.magnitude < axeUnit.StartAttackDistance)
            {
                axeUnit.ChangeState(AxeUnitStateId.Attack);
            }
        }
    }
}
