using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    public void MoveToPlayer()
    {
        _navMeshAgent.SetDestination(PlayerTracker.instance.playerTransform.position);
    }
}
