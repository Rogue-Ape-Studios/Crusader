using System;
using System.Collections.Generic;
using System.Threading;
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

        private CancellationTokenSource _tokenSource = new();

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
                    OnWaveComplete?.Invoke();
                }
            }
        }

        internal Wave CurrentWave => _waves[_currentWave];

        internal Vector3 SpawnPosition => _spawnLocations[UnityEngine.Random.Range(0, _spawnLocations.Count)].position;

        public Vector3 BeginTangent
        {
            set {_beginTangent = value;}
            get { return _beginTangent;}
        }

        public Vector3 EndTangent
        {
            set {_endTangent = value;}
            get { return _endTangent;}
        }

        public Transform SpawnHolder => _spawnLocationHolder;

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
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }

        #endregion

        #region Public

        public void AddSpawn()
        {
            _spawnLocations.Add(Instantiate(_spawnLocations[0],_spawnLocationHolder));
        }

        public void DestroyLastSpawn()
        {
            DestroyImmediate(_spawnLocations[_spawnLocations.Count - 1].gameObject);
            _spawnLocations.RemoveAt(_spawnLocations.Count - 1);
        }

        public int GetSpawnCount()
        {
            return _spawnLocations.Count + 1;
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
                                            SpawnPosition,
                                            Quaternion.identity,
                                            _waveHolder);

                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.enabled = true;
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
                                            SpawnPosition,
                                            Quaternion.identity,
                                            _waveHolder);
                    enemy.OnDeath += HandleEnemyDeath;
                    enemy.enabled = true;
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
            NextWave();
        }

        private void HandleLevelComplete()
        {
            Disable();
        }

        private void OnDrawGizmos()
        {
            foreach (var spawns in _spawnLocations)
            {
                UnityEditor.Handles.DrawBezier(transform.position, spawns.position, _beginTangent, _endTangent, Color.red, null, 2f);
            }
        }

        #endregion
    }
}