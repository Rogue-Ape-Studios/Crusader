using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio
{
    public class Javelin : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Collision _target;
        private void OnCollisionEnter(Collision collision)
        {
            //StickOn(collision);
            Invoke(nameof(DestroyThis), 1f);
        }

        private void DestroyThis()
        {            
            Destroy(gameObject);
        }

        //Disscus if we need stick to projectile 
        private void StickOn(Collision collision)
        {
            gameObject.transform.SetParent(collision.transform, true);
            _rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        }
    }
}
