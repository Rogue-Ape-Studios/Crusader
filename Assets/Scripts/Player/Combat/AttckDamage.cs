using RogueApeStudio.Crusader.HealthSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RogueApeStudio
{
    public class AttckDamage : MonoBehaviour
    {
        public float _damageAmount = 10f;
        [SerializeField] string[] _tags;

        private void OnTriggerEnter(Collider other)
        {
            if (_tags.Any(tag => other.CompareTag(tag)))
            {
                // Check if the other object has a Health component or similar
                Health healthComponent = other.GetComponent<Health>();

                if (healthComponent != null)
                {
                    healthComponent.Hit(_damageAmount);
                }
            }
        }
    }
}
