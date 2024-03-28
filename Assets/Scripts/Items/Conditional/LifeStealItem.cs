using System;
using RogueApeStudio.Crusader.HealthSystem;
using RogueApeStudio.Crusader.Player.Combat;
using UnityEngine;

namespace RogueApeStudio.Crusader.Items.Conditional
{
    public class LifeStealItem : ConditionalItem
    {
        [SerializeField] private Health _playerHealth;
        [Header("Item Settings")]
        [SerializeField, Tooltip("The value to modify the damage by, before applying the lifesteal")]
        private float _damageModifier;
        internal override void SetupCondition()
        {
            if (_conditionTarget == null)
            {
                throw new ArgumentNullException(nameof(_conditionTarget));
            }

            var basicAttack = _conditionTarget as AttackDamage;
            basicAttack.OnDealDamage += HandleConditionMet;
        }

        private void HandleConditionMet(float damage)
        {
            base.HandleConditionMet();
            ConditionalItemEffect(damage);
        }

        internal void ConditionalItemEffect(float damage)
        {
            if(_playerHealth == null)
            {
                throw new ArgumentNullException(nameof(_playerHealth));
            }

            _playerHealth.Heal(damage * _damageModifier * ItemCount);
        }
    }
}
