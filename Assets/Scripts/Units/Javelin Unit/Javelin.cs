using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio
{
    public class Javelin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collision _target;
        [SerializeField] private float _despawnDuration = 1f;
        [SerializeField] private float _playerLayer = 9;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer ==  _playerLayer)
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
