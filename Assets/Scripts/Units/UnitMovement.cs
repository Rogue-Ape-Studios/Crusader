using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RogueApeStudios.Crusader.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private float _aimTurnSpeed = 5;

        public Transform _playerTransform;

        public void MoveToPlayer()
        {
            _navMeshAgent.SetDestination(_playerTransform.position);
        }
        public void SetStopDistance(float stoppingdistance)
        {
            _navMeshAgent.stoppingDistance = stoppingdistance;
        }
        public void SetSpeed(float speed)
        {
            _navMeshAgent.speed = speed;
        }
        public void StopMoving()
        {
            _navMeshAgent.SetDestination(transform.position);
        }
        public void FacePlayer()
        {
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _aimTurnSpeed);
        }

    }
}