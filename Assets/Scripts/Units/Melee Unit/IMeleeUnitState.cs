using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMeleeUnitState
{
    public void EnterState(MeleeUnit meleeUnit);
    public void UpdateState(MeleeUnit meleeUni);
    public void ExitState(MeleeUnit meleeUnit);
}
