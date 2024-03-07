using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnitChaseState : IJavelinUnitState
    {
        public void EnterState(JavelinUnit javelinUnit)
        {
            //start run animation here when available
        }

        public JavelinUnitStateId GetId()
        {
            return JavelinUnitStateId.Chase;
        }

        public void UpdateState(JavelinUnit javelinUnit)
        {
            javelinUnit.LocalUnitMovement.MoveToPlayer();
            Vector3 playerPosition = javelinUnit.LocalUnitMovement._playerTransform.position;
            Vector3 unitPosition = javelinUnit.transform.position;
            
            if (IsInRange(playerPosition, unitPosition, javelinUnit.StartAttackDistance)
                && HasLineOfSight(playerPosition, unitPosition, javelinUnit.AttackRange))
            {
                javelinUnit.LocalUnitMovement.StopMoving();
                javelinUnit.ChangeState(JavelinUnitStateId.Attack);
            }
        }

        private bool IsInRange(Vector3 playerPosition, Vector3 unitPosition, float range)
        {
            Vector3 vectorDistance = playerPosition - unitPosition;
            return vectorDistance.magnitude <= range;

        }

        private bool HasLineOfSight(Vector3 playerPosition, Vector3 unitPosition, float attackRange)
        {
            RaycastHit hit;
            Vector3 playerDirection = playerPosition - unitPosition;
            Debug.DrawLine(playerPosition, playerDirection, Color.red);
            if (Physics.Raycast(unitPosition, playerDirection, out hit, attackRange, ~LayerMask.GetMask("Character")))
            {
                Debug.DrawLine(unitPosition, playerDirection);
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
