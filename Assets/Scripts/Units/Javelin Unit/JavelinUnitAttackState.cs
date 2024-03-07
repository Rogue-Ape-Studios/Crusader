using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public class JavelinUnitAttackState : IJavelinUnitState 
    {
        private float _timePast = 0;

        public void EnterState(JavelinUnit javelinUnit)
        {
            javelinUnit.LocalAnimator.SetTrigger("Attack");
        }

        public JavelinUnitStateId GetId()
        {
            return JavelinUnitStateId.Attack;
        }

        public void UpdateState(JavelinUnit javelinUnit)
        {
            javelinUnit.transform.LookAt(javelinUnit.LocalUnitMovement._playerTransform.position);
            if (Timer(javelinUnit.AttackCooldown))
            {
                Attack(javelinUnit);
            }
            if(!javelinUnit.HasLineOfSight() || !javelinUnit.IsInRange())
            {
                javelinUnit.ChangeState(JavelinUnitStateId.Chase);
            }
        }

        private void Attack(JavelinUnit javelinUnit)
        {
            Rigidbody rb = Object.Instantiate(javelinUnit.Projectile, javelinUnit.ProjectileSpawn.transform.position, javelinUnit.transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(javelinUnit.transform.forward * javelinUnit.ProjectileForce, ForceMode.Impulse);
        }

        private bool Timer(float cooldown)
        {
            _timePast += Time.deltaTime;
            if(_timePast < cooldown)
            {
                return false;
            }
            else
            {
                _timePast = 0;
                return true;
            }
        }
    }
}
