using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public class AxeUnit : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _stopDistance = 1.8f;
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField] private float _startAttackDistance = 2.0f;
        [Header("Attack settings")]
        [SerializeField] private float _damageAmount = 10f;
        [Header("Dependancies")]
        [SerializeField] private UnitMovement _localUnitMovement;
        [SerializeField] private Animator _localAnimator;
        [SerializeField] private Axe _axe;

        private IAxeUnitState _currentState;
        private IAxeUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;


        private void Awake()
        {
            int statesAmount = Enum.GetNames(typeof(AxeUnitStateId)).Length;
            _states = new IAxeUnitState[statesAmount];
            AddAxeUnitState(new AxeUnitChaseState());
            AddAxeUnitState(new AxeUnitAttackState());
            _currentState = GetAxeUnitState(AxeUnitStateId.Chase);
            _currentState.EnterState(this);
            LocalUnitMovement.SetStopDistance(_stopDistance);
            LocalUnitMovement.SetSpeed(_movementSpeed);
            _axe.SetDamageAmount(_damageAmount);
        }

        void Update()
        {
            _currentState.UpdateState(this);
        }

        private void AddAxeUnitState(IAxeUnitState state)
        {
            int index = (int)state.GetId();
            _states[index] = state;
        }

        internal IAxeUnitState GetAxeUnitState(AxeUnitStateId stateId)
        {
            int index = (int)stateId;
            return _states[index];
        }

        internal void ChangeState(AxeUnitStateId stateId)
        {
            _currentState = GetAxeUnitState(stateId);
            _currentState.EnterState(this);
        }
        internal bool IsInRange()
        {
            Vector3 vectorDistance = LocalUnitMovement._playerTransform.position - transform.position;
            return vectorDistance.magnitude <= StartAttackDistance;
        }

        #region Animation Events
        internal void TurnOnAxeHitbox()
        {
            _axe.TurnOnHitbox();
        }
        internal void TurnOffAxeHitbox()
        {
            _axe.TurnOffHitbox();
        }
        #endregion
    }
}
