using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Spawn
{
    [Serializable]
    public struct Wave
    {
        [Header("Spawn Settings")]
        [SerializeField] private bool _randomizeSpawns;
        [SerializeField] private List<EnemySet> _enemies;
        [Header("Time Settings")]
        [SerializeField] private float _timeBetweenSpawns;

        internal readonly bool RandomizedSpawns => _randomizeSpawns;
        internal readonly List<EnemySet> Enemies => _enemies;
        internal readonly float TimeBetweenSpawns => _timeBetweenSpawns;
        
    }
}