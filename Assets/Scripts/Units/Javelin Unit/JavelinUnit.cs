using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnit : MonoBehaviour
    {
        [Header("Movement settings")]
        [SerializeField] private float _stopDistance = 1.8f;
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField] private float _startAttackDistance = 50.0f;
        [Header("Attack settings")]
        [SerializeField] private float _attackRange = 500.0f;
        [SerializeField] private Vector2 _projectileForce = new(32, 2);
        [Header("Line Of Sight Seetings")]
        [SerializeField] private int _amountOfRayCasts = 3;
        [SerializeField] private float _unitWidth = 1f;
        [SerializeField] private float _secondsLosNeeded = 1f;
        [Header("Dependencies")]
        [SerializeField] private UnitMovement _localUnitMovement;
        [SerializeField] private Animator _localAnimator;
        [SerializeField] private GameObject _projectile;
        [SerializeField] private Transform _projectileSpawn;

        private IJavelinUnitState _currentState;
        private IJavelinUnitState[] _states;

        public UnitMovement LocalUnitMovement => _localUnitMovement;
        public Animator LocalAnimator => _localAnimator;
        public float StartAttackDistance => _startAttackDistance;
        public float AttackRange => _attackRange;
        public GameObject Projectile => _projectile;
        public Transform ProjectileSpawn => _projectileSpawn;
        public Vector2 ProjectileForce => _projectileForce;
        public float SecondsLosNeeded => _secondsLosNeeded;


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
            for (int i = 0; i < _amountOfRayCasts; i++)
            {
                Vector3 playerPosition = LocalUnitMovement._playerTransform.position;
                Vector3 unitPosition = transform.position;
                float raycastX = (_unitWidth / (_amountOfRayCasts -1) * i) - (_unitWidth / 2);
                Vector3 raycastPosition = Quaternion.AngleAxis(transform.rotation.eulerAngles.y, Vector3.up) * new Vector3( raycastX, 0, 0);
                raycastPosition = raycastPosition + unitPosition;
                Vector3 playerDirection = playerPosition - raycastPosition;
                RaycastHit hit;
                if (Physics.Raycast(raycastPosition, playerDirection, out hit, AttackRange, ~LayerMask.GetMask("Character")))
                {
                    Debug.DrawLine(raycastPosition, playerPosition, Color.red);
                    if (hit.collider.gameObject.layer != LocalUnitMovement._playerTransform.gameObject.layer)
                    { return false; }
                }
                else { return false; }
            }
            return true;
        }
        #endregion

        #region Animation Events

        public void SpawnProjectile()
        {
            Rigidbody rb = Instantiate(Projectile, ProjectileSpawn.transform.position, transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * ProjectileForce.x, ForceMode.Impulse);
            rb.AddForce(transform.up * ProjectileForce.y, ForceMode.Impulse);
        }

        #endregion
    }
}
