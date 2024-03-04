using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public class MeleeUnit : MonoBehaviour
    {
        public UnitMovement LocalUnitMovement;
        public Animator LocalAnimator;

        public float StartAttackDistance = 2.0f;
        [SerializeField] private float movementSpeed = 3.5f;

        public IMeleeUnitState CurrentState;
        public IMeleeUnitState ChaseState = new MeleeUnitChaseState();
        public IMeleeUnitState AttackState = new MeleeUnitAttackState();   

        private void Awake()
        {
            CurrentState = ChaseState;
            LocalUnitMovement.SetStopDistance(StartAttackDistance - 0.2f);
            LocalUnitMovement.SetSpeed(movementSpeed);
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
}
