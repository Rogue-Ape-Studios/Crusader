using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using RogueApeStudio.Crusader.HealthSystem;
using UnityEngine;

namespace RogueApeStudio.Crusader.Spawn
{
    public class SpawnManager : MonoBehaviour
    {
        #region Events
        
        public event Action OnWaveComplete;
        public event Action OnLevelComplete;

        #endregion

        #region Serialized Fields

        [SerializeField] private List<Transform> _spawnLocations;
        [SerializeField] private List<Health> _enemies;
        [SerializeField] private List<Wave> _waves;
        [SerializeField] private Transform _waveHolder;
        [SerializeField, Range(0, 50)] private int _minimumEnemiesThreshold = 5;

        #endregion

        #region Fields

        private int _currentWave = 0;
        private int _remainingEnemies;

        #endregion

        #region Properties

        internal int RemainingEnemies
        {
            get
            {
                return _remainingEnemies;
            }
            private set
            {
                _remainingEnemies = value;

                if (_remainingEnemies < _minimumEnemiesThreshold)
                {
                    NextWave();
                    OnWaveComplete?.Invoke();
                }
            }
        }

        internal Wave CurrentWave => _waves[_currentWave];

        internal Vector3 SpawnPosition => _spawnLocations[UnityEngine.Random.Range(0, _spawnLocations.Count)].position;

        #endregion

        #region Unity Methods

        private void Start()
        {
            OnWaveComplete += HandleWaveComplete;
            OnLevelComplete += HandleLevelComplete;
            RemainingEnemies = 0;
        }

        private void OnDestroy()
        {
            OnWaveComplete -= HandleWaveComplete;
        }

        #endregion

        #region Internal

        internal async void NextWave()
        {
            if (_currentWave >= _waves.Count)
            {
                OnLevelComplete?.Invoke();
                return;
            }

            await StandardizedSpawnsAsync();
        }


        #endregion

        #region Private

        private async UniTask StandardizedSpawnsAsync()
        {
            foreach (var enemySet in _waves[_currentWave].Enemies)
            {
                for (var i = 0; i < enemySet.Count; i++)
                {
                    var enemy = Instantiate(enemySet.EnemyPrefab,
                                            SpawnPosition,
                                            Quaternion.identity,
                                            _waveHolder);

                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.enabled = true;
                }
                await UniTask.WaitForSeconds(_waves[_currentWave].TimeBetweenSpawns);
            }
        }

        private void Disable()
        {
            enabled = false;
        }

        private void HandleEnemyDeath()
        {
            RemainingEnemies--;
        }

        private void HandleWaveComplete()
        {
            _waves.RemoveAt(_currentWave);
            NextWave();
        }

        private void HandleLevelComplete()
        {
            Disable();
        }

        #endregion
    }
}