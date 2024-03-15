using RogueApeStudio.Crusader.HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{   
    [SerializeField] Collider _axeHitBox;
    private float _damageAmount;

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