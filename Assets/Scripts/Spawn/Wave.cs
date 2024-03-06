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

        /// <summary>
        /// Are the spawns randomized (Grab a random amount of enemies from the pool in the wave.)
        /// </summary>
        internal readonly bool RandomizedSpawns => _randomizeSpawns;
        /// <summary>
        /// The list of enemies in a wave.
        /// </summary>
        internal readonly List<EnemySet> Enemies => _enemies;
        /// <summary>
        /// The time between spawns in seconds.
        /// </summary>
        internal readonly float TimeBetweenSpawns => _timeBetweenSpawns;
        
    }
}