using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueApeStudio.Crusader.Units.MeleeUnit
{
    public interface IMeleeUnitState
    {
        public void EnterState(MeleeUnit meleeUnit);
        public void UpdateState(MeleeUnit meleeUni);
        public void ExitState(MeleeUnit meleeUnit);
    }
}