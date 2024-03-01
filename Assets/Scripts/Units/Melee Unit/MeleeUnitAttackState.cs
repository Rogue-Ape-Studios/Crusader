using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitAttackState : IMeleeUnitState
{
    public void EnterState(MeleeUnit meleeUnit)
    {

    }

    public void ExitState(MeleeUnit meleeUnit)
    {
        
    }

    public void UpdateState(MeleeUnit meleeUnit)
    {
        Debug.Log("Attack player!");
        meleeUnit.ChangeState(meleeUnit.ChaseState);
    }
}
