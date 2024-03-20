using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
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

        [Header("Spawn Settings")]
        [SerializeField] private Transform _spawnLocationHolder;
        [SerializeField] private List<Transform> _spawnLocations;

        [Header("Wave Settings")]
        [SerializeField] private List<Wave> _waves;
        [SerializeField] private Transform _waveHolder;
        [SerializeField, Range(1, 50)] private int _minimumEnemiesThreshold = 5;

        private Vector3 _beginTangent = new(0, 0, 0);
        private Vector3 _endTangent = new(0, 20, 0);

        #endregion

        #region Fields

        private int _currentWave = 0;
        private int _remainingEnemies;

        private readonly CancellationTokenSource _tokenSource = new();

        #endregion

        #region Properties

        /// <summary>
        /// The number of remaining enemies.
        /// </summary>
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
                    OnWaveComplete?.Invoke();
                }
            }
        }

        /// <summary>
        /// The current wave.
        /// </summary>
        internal Wave CurrentWave => _waves[_currentWave];

        /// <summary>
        /// The begin tangent for the Bezier curve.
        /// </summary>
        public Vector3 BeginTangent
        {
            set { _beginTangent = value; }
            get { return _beginTangent; }
        }

        /// <summary>
        /// The End Tangent for the Bezier curve.
        /// </summary>
        public Vector3 EndTangent
        {
            set { _endTangent = value; }
            get { return _endTangent; }
        }

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
            OnLevelComplete -= HandleLevelComplete;
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        #endregion

        #region Public

        /// <summary>
        /// Add a spawn to the list.
        /// </summary>
        public void AddSpawn()
        {
            _spawnLocations.Add(Instantiate(_spawnLocations[0], _spawnLocationHolder));
        }

        /// <summary>
        /// Destroy the last spawn in the list.
        /// </summary>
        public void DestroyLastSpawn()
        {
            if (_spawnLocations.Count <= 1)
            {
                Debug.LogError("Must have at least 1 spawnpoint!");
                return;
            }
            DestroyImmediate(_spawnLocations[_spawnLocations.Count - 1].gameObject);
            _spawnLocations.RemoveAt(_spawnLocations.Count - 1);
        }

        /// <summary>
        /// Get the last spawn ID.
        /// </summary>
        /// <returns>The last spawn ID, plus 1.</returns>
        public int GetSpawnCount()
        {
            return _spawnLocations.Count + 1;
        }

        #endregion

        #region Internal

        /// <summary>
        /// Triggers the next wave to be spawned, or indicates that the level is complete.
        /// </summary>
        internal async void NextWaveAsync()
        {
            if(!enabled) return;
            
            if (_currentWave >= _waves.Count)
            {
                OnLevelComplete?.Invoke();
                return;
            }

            try
            {
                if (!CurrentWave.RandomizedSpawns)
                {
                    await StandardizedSpawnsAsync(_tokenSource.Token);
                }
                else
                {
                    await RandomizedSpawnsAsync(_tokenSource.Token);
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.LogError("Spawning a wave was cancelled, because the operation was cancelled!\n" + ex.Message);
            }

            _currentWave++;
        }


        #endregion

        #region Private

        private async UniTask StandardizedSpawnsAsync(CancellationToken token)
        {
            foreach (var enemySet in CurrentWave.Enemies)
            {
                for (var i = 0; i < enemySet.Count; i++)
                {
                    var enemy = Instantiate(enemySet.EnemyPrefab,
                                            GetRandomSpawn(),
                                            Quaternion.identity,
                                            _waveHolder);

                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.gameObject.SetActive(true);
                    _remainingEnemies++;
                }
                await UniTask.Delay(TimeSpan.FromSeconds(CurrentWave.TimeBetweenSpawns), cancellationToken: token);
            }
        }

        private async UniTask RandomizedSpawnsAsync(CancellationToken token)
        {
            int enemyIndex = 0, enemyCount;

            while (CurrentWave.Enemies.Count != 0)
            {
                enemyIndex = UnityEngine.Random.Range(0, CurrentWave.Enemies.Count);
                enemyCount = UnityEngine.Random.Range(0, CurrentWave.Enemies[enemyIndex].Count + 1);

                for (int i = 0; i < enemyCount; i++)
                {
                    var enemy = Instantiate(CurrentWave.Enemies[enemyIndex].EnemyPrefab,
                                            GetRandomSpawn(),
                                            Quaternion.identity,
                                            _waveHolder);
                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.gameObject.SetActive(true);
                    _remainingEnemies++;
                }

                CurrentWave.Enemies[enemyIndex].ReduceCount(enemyCount);

                if (CurrentWave.Enemies[enemyIndex].Count == 0)
                {
                    CurrentWave.Enemies.RemoveAt(enemyIndex);
                }

                await UniTask.Delay(TimeSpan.FromSeconds(CurrentWave.TimeBetweenSpawns), cancellationToken: token);
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
            NextWaveAsync();
        }

        private void HandleLevelComplete()
        {
            Disable();
        }

        private Vector3 GetRandomSpawn()
        {
            int randomVal = UnityEngine.Random.Range(0, _spawnLocations.Count);
            try
            {
                return _spawnLocations[randomVal].position;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Debug.LogError("The value for the spawn positions was out of range!Value: " + randomVal + "\n" + ex.Message);
            }

            return _spawnLocations[0].position;
        }

        #endregion

        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            foreach (var spawns in _spawnLocations)
            {
                UnityEditor.Handles.DrawBezier(transform.position, spawns.position, _beginTangent, _endTangent, Color.red, null, 2f);
            }
        }

        #endif
    }
}