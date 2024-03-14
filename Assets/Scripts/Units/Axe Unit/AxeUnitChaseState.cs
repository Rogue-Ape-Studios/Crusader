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
            Vector3 vectordistance = axeUnit.LocalUnitMovement._playerTransform.position - axeUnit.transform.position;
            if (vectordistance.magnitude < axeUnit.StartAttackDistance)
            {
                axeUnit.ChangeState(AxeUnitStateId.Attack);
            }
        }
    }
}
