using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float _damageAmount;
    [SerializeField] Collider _axeHitBox;

    private void OnTriggerEnter(Collider other)
    {
        Damage(other);
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