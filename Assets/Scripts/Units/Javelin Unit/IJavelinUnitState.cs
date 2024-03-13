using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudios.Crusader.Units.JavelinUnit
{
    public enum JavelinUnitStateId
    {
        Chase,
        Attack
    }

    public interface IJavelinUnitState
    {
        public JavelinUnitStateId GetId();
        public void EnterState(JavelinUnit axeUnit);
        public void UpdateState(JavelinUnit axeUnit);
    }
}