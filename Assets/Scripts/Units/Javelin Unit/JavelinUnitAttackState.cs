using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
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
            javelinUnit.ChangeState(JavelinUnitStateId.Chase);
        }
    }
}
