using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.JavelinUnit
{
    public enum JavelinUnitStateId
    {
        Chase,
        Attack,
        Death,
        Stunned
    }

    public interface IJavelinUnitState
    {
        public JavelinUnitStateId GetId();
        public void EnterState(JavelinUnit javelinUnit);
        public void UpdateState(JavelinUnit javlinUnit);
    }
}