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

        /// <summary>
        /// Reduce the number of enemies in the wave.
        /// </summary>
        /// <param name="reduceCountBy">The value to reduce the remaining count by.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a value is greater than itself.</exception>
        public void ReduceCount(int reduceCountBy)
        {
            if(reduceCountBy > _count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(reduceCountBy),
                    "Cannot reduce the amount of enemies to spawn by a value greater than itself!");
            }

            if(reduceCountBy < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(reduceCountBy),
                    "Cannot reduce by a value less than zero!");
            }

            _count -= reduceCountBy;
        }
    }
}