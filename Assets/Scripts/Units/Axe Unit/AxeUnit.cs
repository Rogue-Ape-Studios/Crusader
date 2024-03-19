using RogueApeStudio.Crusader.HealthSystem;
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
        [SerializeField] private float _stopAttackDistance = 3.0f;
        [Header("Attack settings")]
        [SerializeField] private float _damageAmount = 10f;
        [SerializeField] private float _attackSpeed = 1;
        [Header("Death settings")]
        [SerializeField] private float _destroyTime = 3;
        [Header("Dependancies")]
        [SerializeField] private UnitMovement _localUnitMovement;
        [SerializeField] private Animator _localAnimator;
        [SerializeField] private Axe _axe;
        [SerializeField] private Health _health;
        [SerializeField] private Collider _collider;

        private IAxeUnitState _currentState;
        private IAxeUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;
        public float StopAttackDistance => _stopAttackDistance;
        public float DestroyTime => _destroyTime;


        private void Awake()
        {
            int statesAmount = Enum.GetNames(typeof(AxeUnitStateId)).Length;
            _states = new IAxeUnitState[statesAmount];
            AddAxeUnitState(new AxeUnitChaseState());
            AddAxeUnitState(new AxeUnitAttackState());
            AddAxeUnitState(new AxeUnitDeathState());
            _currentState = GetAxeUnitState(AxeUnitStateId.Chase);
            _currentState.EnterState(this);
            LocalUnitMovement.SetStopDistance(_stopDistance);
            LocalUnitMovement.SetSpeed(_movementSpeed);
            LocalAnimator.SetFloat("Attack Speed", _attackSpeed);
            _axe.SetDamageAmount(_damageAmount);
            _health.OnDeath += HandleDeath;
        }
        private void HandleDeath()
        {
            ChangeState(AxeUnitStateId.Death);
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

        internal float PlayerDistance()
        {
            Vector3 vectorDistance = LocalUnitMovement.PlayerTransform.position - transform.position;
            return vectorDistance.magnitude;
        }

        internal void DestroySelf()
        {
            Destroy(gameObject);
        }

        internal void TurnOffCollider()
        {
            _collider.enabled = false;
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
