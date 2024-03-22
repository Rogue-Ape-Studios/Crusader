using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RogueApeStudio.Crusader.HealthSystem
{
    public class Health : MonoBehaviour
    {
        #region Events

        public event Action OnDeath;
        public event Action OnHeal;
        public event Action<float> OnDamage;
        public event Action OnRegeneration;

        #endregion

        #region Serialized Fields

        [Header("Health Bar Properties")]
        [SerializeField] protected float _maxHealth = 100f;
        [SerializeField] protected float _currentHealth;
        [Header("Regeneration Stats")]
        [SerializeField] protected bool _canRegenerate;
        [SerializeField, Tooltip("Will be replaced with stats later.")] protected float _regenerationAmount;
        [SerializeField, Tooltip("Will be replaced with stats later.")] protected float _regenerationTickDelaySeconds;
        [SerializeField, Tooltip("Will be replaced with stats later.")] protected float _initalRegenerationDelaySeconds;

        #endregion

        #region Fields

        private bool _isRegenerating = false;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion
 
        #region Properties

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        #endregion

        #region Unity Start

        private void Start()
        {
            _currentHealth = MaxHealth;
            OnRegeneration += HandleHealthRegeneration;
            _cancellationTokenSource = new();
        }

        private void OnDestroy()
        {
            OnDamage = null;
            OnHeal = null;
            OnDamage = null;
            OnRegeneration = null;

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion

        #region Public Methods

        public void Hit(float damage)
        {
            _currentHealth -= damage;
            StopRegeneration();
            
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnDeath?.Invoke();
            }
            else
            {
                OnDamage?.Invoke(damage);
                
                if(!_isRegenerating && _canRegenerate)
                {
                    StartRegnerationAsync();
                }
            }
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
                StopRegeneration();
            }
            OnHeal?.Invoke();
        }

        public async void HandleHealthRegeneration()
        {
            Heal(_regenerationAmount);
            await StartRegnerationAsync();
        }

        #endregion

        #region Private

        private async UniTask StartRegnerationAsync()
        {
            if(!_isRegenerating)
            {
                _isRegenerating = true;
                await UniTask.WaitForSeconds(_initalRegenerationDelaySeconds, cancellationToken: _cancellationTokenSource.Token);
            }

            await UniTask.WaitForSeconds(_regenerationTickDelaySeconds, cancellationToken: _cancellationTokenSource.Token);
            OnRegeneration?.Invoke();
        }

        private void StopRegeneration()
        {
            _isRegenerating = false;
        }

        #endregion
    }
}