using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class AxeUnit : MonoBehaviour
    {
        public UnitMovement LocalUnitMovement;
        public Animator LocalAnimator;

        public float StartAttackDistance = 2.0f;
        [SerializeField] private float _stopDistance = 1.8f;
        [SerializeField] private float _movementSpeed = 3.5f;

        public IAxeUnitState CurrentState;
        public IAxeUnitState ChaseState = new AxeUnitChaseState();
        public IAxeUnitState AttackState = new AxeUnitAttackState();

        private void Awake()
        {
            CurrentState = ChaseState;
            LocalUnitMovement.SetStopDistance(_stopDistance);
            LocalUnitMovement.SetSpeed(_movementSpeed);
        }

        void Update()
        {
            CurrentState.UpdateState(this);
        }

        public void ChangeState(IAxeUnitState newState)
        {
            CurrentState = newState;
            CurrentState.EnterState(this);
        }
    }
}
