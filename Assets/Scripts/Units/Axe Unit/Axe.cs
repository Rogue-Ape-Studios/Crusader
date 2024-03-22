using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] Collider _axeHitBox;
    [SerializeField] private string[] _tags;
    private float _damageAmount;

    private void OnTriggerEnter(Collider other)
    {
        if (_tags.Any(tag => other.transform.root.CompareTag(tag)))
        {
            Damage(other);
        }
    }

    private void Damage(Collider other)
    {
            Health health = other.GetComponentInParent<Health>();
            health.Hit(_damageAmount);
    }

    internal void SetDamageAmount(float damageAmount)
    {
        _damageAmount = damageAmount;
    }

    internal void TurnOnHitbox()
    {
        _axeHitBox.enabled = true;
    }

    internal void TurnOffHitbox()
    {
        _axeHitBox.enabled = false;
    }
}