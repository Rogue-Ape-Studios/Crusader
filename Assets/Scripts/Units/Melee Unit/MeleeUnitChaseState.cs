using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        meleeUnit.UnitMovement.MoveToPlayer();
        Vector3 vectordistance = PlayerTracker.instance.playerTransform.position - meleeUnit.transform.position;
        if (vectordistance.magnitude < meleeUnit.StartAttackDistance)
        {
            meleeUnit.ChangeState(meleeUnit.AttackState);
        }
    }
}
