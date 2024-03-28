using RogueApeStudio.Crusader.HealthSystem;
using RogueApeStudio.Crusader.Items;
using RogueApeStudio.Crusader.Units.Knockback;
using System;
using System.Linq;
using UnityEngine;

namespace RogueApeStudio.Crusader.Player.Combat
{
    public class AttackDamage : MonoBehaviour
    {
        public event Action<float> OnDealDamage;
        [SerializeField] private float _damageAmount => _playerStats.AttackDamage;
        [SerializeField] private string[] _tags;
        [SerializeField] private bool _applyKnockback = false;
        [SerializeField] private float _force;
        [Header("Dependencies")]
        [SerializeField] private PlayerScriptableObject _playerStats;

        private void OnTriggerEnter(Collider other)
        {
            if (_tags.Any(tag => other.CompareTag(tag)))
            {
                // Check if the other object has a Health component or similar
                Health healthComponent = other.GetComponent<Health>();
                Knockback knockbackComponent = other.GetComponent<Knockback>();

                if (healthComponent != null)
                {
                    healthComponent.Hit(_damageAmount);
                    OnDealDamage?.Invoke(_damageAmount);
                }

                if (_applyKnockback && knockbackComponent != null)
                {
                    Vector3 knockbackDiraction = transform.position - other.transform.position;
                    knockbackDiraction = knockbackDiraction.normalized;
                    knockbackComponent.AddKnockback(_force, -knockbackDiraction);
                }
            }
        }
    }
}
