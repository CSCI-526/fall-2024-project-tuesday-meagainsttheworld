using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDisappear : Trap
{
    public override void ActivateSkill()
    {
        Debug.Log($"Trap {name} activates its skill: disappear");
        Disappear();
    }

    public override void DeactivateSkill()
    {
        Debug.Log($"Trap {name} deactivates its skill: reappear");
        Reappear();
    }
}
