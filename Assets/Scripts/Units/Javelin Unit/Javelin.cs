using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class Javelin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collision _target;
        [SerializeField] private float _despawnDuration = 1f;
        [SerializeField] private float _damageAmount = 5f;
        [SerializeField] private LayerMask _playerLayer;

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == _playerLayer.value)
            {
                Damage(other);
                DestroyThis();
            }
            else
            {
                Stick();
                Invoke(nameof(DestroyThis), _despawnDuration);
            }
        }

        private void DestroyThis()
        {            
            Destroy(gameObject);
        }

        private void Stick()
        {
            _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }

        private void Damage(Collider other)
        {
            Health health = other.GetComponentInParent<Health>();
            health.Hit(_damageAmount);
        }
    }
}
