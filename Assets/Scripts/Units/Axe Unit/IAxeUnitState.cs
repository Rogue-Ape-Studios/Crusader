using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.AxeUnit
{
    public enum AxeUnitStateId
    {
        Chase,
        Attack
    }

    public interface IAxeUnitState
    {
        public AxeUnitStateId GetId();
        public void EnterState(AxeUnit axeUnit);
        public void UpdateState(AxeUnit axeUnit);
    }
}