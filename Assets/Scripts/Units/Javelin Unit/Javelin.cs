using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudios.Crusader.Units.JavelinUnit
{
    public class Javelin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collision _target;
        [SerializeField] private float _despawnDuration = 1f;
        [SerializeField] private LayerMask _playerLayer;

        private void Awake()
        {
            _playerLayer = LayerMask.NameToLayer("Player");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer ==  _playerLayer.value)
            {
                //deal damage
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
    }
}
