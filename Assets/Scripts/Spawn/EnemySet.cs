using System;
using UnityEngine;
using RogueApeStudio.Crusader.HealthSystem;

namespace RogueApeStudio.Crusader.Spawn
{
    [Serializable]
    public class EnemySet
    {
        [SerializeField] private Health _prefab;
        [SerializeField] private int _count;

        public int Count => _count;
        public Health EnemyPrefab => _prefab;
    }
}