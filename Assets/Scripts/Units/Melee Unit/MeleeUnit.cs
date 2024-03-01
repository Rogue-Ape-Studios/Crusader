using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnit : MonoBehaviour
{
    public UnitMovement UnitMovement;    
    public IMeleeUnitState CurrentState;
    public IMeleeUnitState ChaseState = new MeleeUnitChaseState();
    public IMeleeUnitState AttackState = new MeleeUnitAttackState();
    public float StartAttackDistance = 2.0f;

    private void Awake()
    {       
        CurrentState = ChaseState;
    }

    void Update()
    {
        CurrentState.UpdateState(this);
    }

    public void ChangeState(IMeleeUnitState newState)
    {
        CurrentState.ExitState(this);
        CurrentState = newState;
        CurrentState.EnterState(this);
    }
}
