using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace RogueApeStudio.Crusader.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;
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
    }
}