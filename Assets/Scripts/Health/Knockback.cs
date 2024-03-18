using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

namespace RogueApeStudio.Crusader.HealthSystem.Knockback
{
    public class Knockback : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Rigidbody _rb;

        public void AddKnockback(float force, Vector3 diraction)
        {
            _rb.isKinematic = false;
            _agent.enabled = false;
            _rb.AddForce(diraction * force, ForceMode.Impulse);
        }

        void FixedUpdate()
        {
            if (_rb.velocity.magnitude <= 0.1f)
            {
                _rb.isKinematic = true;
                _agent.enabled = true;
            }
        }
    }
}
