using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class Javelin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collision _target;
        [SerializeField] private float _despawnDuration = 1f;
        [SerializeField] private float _damageAmount = 5f;
        [SerializeField] private string[] _tags;

        private void Awake()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_tags.Any(tag => other.transform.root.CompareTag(tag)))
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
