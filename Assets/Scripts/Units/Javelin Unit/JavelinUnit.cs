using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnit : MonoBehaviour
    {
        [SerializeField] private float _stopDistance = 1.8f;
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField] private float _startAttackDistance = 50.0f;
        [SerializeField] private UnitMovement _localUnitMovement;
        [SerializeField] private Animator _localAnimator;
        [SerializeField] private float _attackRange = 500.0f;

        private IJavelinUnitState _currentState;
        private IJavelinUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;
        public float AttackRange => _attackRange;


        private void Awake()
        {
            int statesAmount = Enum.GetNames(typeof(JavelinUnitStateId)).Length;
            _states = new IJavelinUnitState[statesAmount];
            AddJavelinUnitState(new JavelinUnitChaseState());
            AddJavelinUnitState(new JavelinUnitAttackState());
            _currentState = GetJavelinUnitState(JavelinUnitStateId.Chase);
            LocalUnitMovement.SetStopDistance(_stopDistance);
            LocalUnitMovement.SetSpeed(_movementSpeed);
        }

        void Update()
        {
            _currentState.UpdateState(this);
        }

        public void AddJavelinUnitState(IJavelinUnitState state)
        {
            int index = (int)state.GetId();
            _states[index] = state;
        }

        public IJavelinUnitState GetJavelinUnitState(JavelinUnitStateId stateId)
        {
            int index = (int)stateId;
            return _states[index];
        }

        public void ChangeState(JavelinUnitStateId stateId)
        {
            _currentState = GetJavelinUnitState(stateId);
            _currentState.EnterState(this);
        }
    }
}
