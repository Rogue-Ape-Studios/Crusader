using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshController : MonoBehaviour
{
    [SerializeField] private float _avoidancePredictionTime = 2.0f;
    [SerializeField] private int _pathfindingIterationsPerFrame = 100;
    private void Awake()
    {
        NavMesh.avoidancePredictionTime = _avoidancePredictionTime;
        NavMesh.pathfindingIterationsPerFrame = _pathfindingIterationsPerFrame;
    }
}
