using System;
using UnityEngine;

namespace RogueApeStudio.Crusader.HealthSystem
{
    public class Health : MonoBehaviour
    {
        #region Events

        public event Action OnDeath;
        public event Action OnHeal;
        public event Action<float> OnDamage;

        #endregion

        #region Serialized Fields

        [Header("Health Bar Properties")]
        [SerializeField] protected float _maxHealth = 100f;
        [SerializeField] protected float _currentHealth;

        #endregion

        #region Properties

        public float MaxHealth => _maxHealth;
        public float CurrentHealth => _currentHealth;

        #endregion

        #region Unity Start

        private void Start()
        {
            _currentHealth = MaxHealth;
        }

        private void OnDestroy()
        {
            OnDamage = null;
            OnHeal = null;
            OnDamage = null;
        }

        #endregion

        #region Public Methods

        public void Hit(float damage)
        {
            _currentHealth -= damage;
            if (_currentHealth < 0)
            {
                _currentHealth = 0;
                OnDeath?.Invoke();
            }
            else
            {
                OnDamage?.Invoke(damage);
            }
        }

        public void Heal(float healAmount)
        {
            _currentHealth += healAmount;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }
            OnHeal?.Invoke();
        }

        #endregion
    }
}