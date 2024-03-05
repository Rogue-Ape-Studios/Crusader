using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public interface IAxeUnitState
    {
        public void EnterState(AxeUnit meleeUnit);
        public void UpdateState(AxeUnit meleeUni);
    }
}