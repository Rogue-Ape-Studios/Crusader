using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RogueApeStudios.Crusader.Units.JavelinUnit
{
    public class JavelinUnitChaseState : IJavelinUnitState
    {
        private float _lineOfSightTime = 0;
        public void EnterState(JavelinUnit javelinUnit)
        {
            //start run animation here when available
        }

        public JavelinUnitStateId GetId()
        {
            return JavelinUnitStateId.Chase;
        }

        public void UpdateState(JavelinUnit javelinUnit)
        {
            javelinUnit.LocalUnitMovement.MoveToPlayer();

            if (LineOfSightTimeHasPassed(javelinUnit))
            {
                javelinUnit.LocalUnitMovement.StopMoving();
                javelinUnit.ChangeState(JavelinUnitStateId.Attack);
            }
        }

        private bool LineOfSightTimeHasPassed(JavelinUnit javelinUnit)
        {
            if (javelinUnit.IsInRange() && javelinUnit.HasLineOfSight())
            {
                _lineOfSightTime += Time.deltaTime;
                if (_lineOfSightTime >= javelinUnit.SecondsLineOfSightsNeeded)
                {
                    return true;
                }
                else { return false; }
            }
            else
            {
                _lineOfSightTime = 0;
                return false;
            }
        }

    }
}
