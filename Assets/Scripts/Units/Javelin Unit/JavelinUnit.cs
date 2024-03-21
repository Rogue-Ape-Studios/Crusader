using RogueApeStudio.Crusader.HealthSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnit : Enemy
    {
        [Header("Movement settings")]
        [SerializeField] private float _stopDistance = 1.8f;
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField] private float _startAttackDistance = 50.0f;
        [Header("Combat settings")]
        [SerializeField] private float _attackRange = 500.0f;
        [SerializeField] private Vector2 _projectileForce = new(32, 2);
        [SerializeField] private float _selfStunDuration = 1;
        [Header("Line Of Sight Seetings")]
        [SerializeField] private int _amountOfRayCasts = 3;
        [SerializeField] private float _unitWidth = 1f;
        [SerializeField] private float _secondsLineOfSightNeeded = 1f;
        [SerializeField] private float _raycastHeight = 1f;
        [SerializeField] private string _playerTag;
        [Header("Death settings")]
        [SerializeField] private float _destroyTime = 3;
        [Header("Dependencies")]
        [SerializeField] private UnitMovement _localUnitMovement;
        [SerializeField] private Animator _localAnimator;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform _projectileSpawn;
        [SerializeField] private Health _health;
        [SerializeField] private Collider _collider;

        private IJavelinUnitState _currentState;
        private IJavelinUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;
        public float AttackRange => _attackRange;
        public GameObject Projectile => _projectile;
        public Transform ProjectileSpawn => _projectileSpawn;
        public Vector2 ProjectileForce => _projectileForce;
        public float SecondsLineOfSightsNeeded => _secondsLineOfSightNeeded;
        public float DestroyTime => _destroyTime;
        public float SelfStunDuration => _selfStunDuration;


        private void Awake()
        {
            int statesAmount = Enum.GetNames(typeof(JavelinUnitStateId)).Length;
            _states = new IJavelinUnitState[statesAmount];
            AddJavelinUnitState(new JavelinUnitChaseState());
            AddJavelinUnitState(new JavelinUnitAttackState());
            AddJavelinUnitState(new JavelinUnitDeathState());
            AddJavelinUnitState(new JavelinUnitStunnedState());
            _currentState = GetJavelinUnitState(JavelinUnitStateId.Chase);
            _currentState.EnterState(this);
            LocalUnitMovement.SetStopDistance(_stopDistance);
            LocalUnitMovement.SetSpeed(_movementSpeed);
            _health.OnDeath += HandleDeath;
        }
        private void HandleDeath()
        {
            ChangeState(JavelinUnitStateId.Death);
        }

        void Update()
        {
            _currentState.UpdateState(this);
        }

        internal void AddJavelinUnitState(IJavelinUnitState state)
        {
            int index = (int)state.GetId();
            _states[index] = state;
        }

        internal IJavelinUnitState GetJavelinUnitState(JavelinUnitStateId stateId)
        {
            int index = (int)stateId;
            return _states[index];
        }

        internal void ChangeState(JavelinUnitStateId stateId)
        {
            _currentState = GetJavelinUnitState(stateId);
            _currentState.EnterState(this);
        }

        #region Public Methods
        internal bool IsInRange()
        {
            Vector3 vectorDistance = LocalUnitMovement.PlayerTransform.position - transform.position;
            return vectorDistance.magnitude <= StartAttackDistance;
        }

        internal bool HasLineOfSight()
        {
            for (int i = 0; i < _amountOfRayCasts; i++)
            {
                Vector3 playerPosition = LocalUnitMovement.PlayerTransform.position;
                playerPosition += new Vector3(0, _raycastHeight, 0);
                Vector3 unitPosition = transform.position;
                float raycastX = (_unitWidth / (_amountOfRayCasts -1) * i) - (_unitWidth / 2);
                Vector3 raycastPosition = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3( raycastX, _raycastHeight, 0);
                raycastPosition = raycastPosition + unitPosition;
                Vector3 playerDirection = playerPosition - raycastPosition;
                RaycastHit hit;
                if (Physics.Raycast(raycastPosition, playerDirection, out hit, AttackRange, ~LayerMask.GetMask("Character")))
                {
                    Debug.DrawLine(raycastPosition, playerPosition, Color.red);
                    if (!hit.collider.gameObject.transform.root.CompareTag(_playerTag))
                    {
                        //Debug.Log("lost");
                        return false; 
                    }
                }
                else 
                { 
                    //Debug.Log("lost");
                    return false; 
                }
            }
            return true;
        }

        internal void DestroySelf()
        {
            Destroy(gameObject);
        }
        internal void TurnOffCollider()
        {
            _collider.enabled = false;
        }
        public override void Stun()
        {
            ChangeState(JavelinUnitStateId.Stunned);
        }

        #endregion

        #region Animation Events

        internal void SpawnProjectile()
        {
            Rigidbody rb = Instantiate(Projectile, ProjectileSpawn.transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * ProjectileForce.x, ForceMode.Impulse);
            rb.AddForce(transform.up * ProjectileForce.y, ForceMode.Impulse);
        }

        #endregion
    }
}
