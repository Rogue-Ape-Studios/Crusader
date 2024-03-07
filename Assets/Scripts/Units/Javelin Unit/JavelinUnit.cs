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
        [SerializeField] private GameObject _projectile;
        [SerializeField] private float _attackCooldown = 2.0f;
        [SerializeField] private Transform _projectileSpawn;
        [SerializeField] private float _projectileForce = 32f;

        private IJavelinUnitState _currentState;
        private IJavelinUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;
        public float AttackRange => _attackRange;
        public GameObject Projectile => _projectile;
        public float AttackCooldown => _attackCooldown;
        public Transform ProjectileSpawn => _projectileSpawn;
        public float ProjectileForce => _projectileForce;


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

        #region Commen Methods
        public bool IsInRange()
        {
            Vector3 vectorDistance = LocalUnitMovement._playerTransform.position - transform.position;
            return vectorDistance.magnitude <= StartAttackDistance;
        }

        public bool HasLineOfSight()
        {
            Vector3 playerPosition = LocalUnitMovement._playerTransform.position;
            Vector3 unitPosition = transform.position;

            RaycastHit hit;
            Vector3 playerDirection = playerPosition - unitPosition;
            Debug.DrawLine(playerPosition, playerDirection, Color.red);
            if (Physics.Raycast(unitPosition, playerDirection, out hit, AttackRange, ~LayerMask.GetMask("Character")))
            {
                Debug.DrawLine(unitPosition, playerDirection);
                if (hit.transform.CompareTag("Player"))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
