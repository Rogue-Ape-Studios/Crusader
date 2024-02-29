using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeUnitBehaviour : MonoBehaviour
{
    [SerializeField] private UnitMovement _unitMovement;
    [SerializeField] private Mode _mode;
    private bool _attackStarted = false;

    enum Mode
    {
        Idle,
        Chase,
        Attack
    }

    private void Awake()
    {
        _mode = Mode.Chase;
    }

    void Update()
    {
        switch (_mode)
        {
            case Mode.Idle:
                //Put wander behaviour here maybe? For now stand still.
                break;
            case Mode.Chase:                
                _unitMovement.MoveToPlayer();
                break;
            case Mode.Attack:
                if (!_attackStarted)
                {
                    _attackStarted = true;
                    Attack();
                }
                    break;
            default:
                throw new NotImplementedException();
                break;
        }
    }

    private void Attack()
    {
        throw new NotImplementedException();
    }

}
