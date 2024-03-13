using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudios.Crusader.Units.JavelinUnit
{
    public class JavelinUnitAttackState : IJavelinUnitState
    {
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
            javelinUnit.LocalUnitMovement.FacePlayer();
            if (!javelinUnit.HasLineOfSight() || !javelinUnit.IsInRange())
            {
                javelinUnit.LocalAnimator.SetTrigger("Chase");
                javelinUnit.ChangeState(JavelinUnitStateId.Chase);
            }
        }
    }
}
