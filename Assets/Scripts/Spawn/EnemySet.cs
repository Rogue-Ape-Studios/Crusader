using System;
using UnityEngine;
using UnityEngine.AI;

namespace RogueApeStudio.Crusader.Spawn
{
    [Serializable]
    public class EnemySet
    {
        [SerializeField] private NavMeshAgent _prefab;
        [SerializeField] private int _count;

        public int Count => _count;
        public NavMeshAgent EnemyPrefab => _prefab;
    }
}