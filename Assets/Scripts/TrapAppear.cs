using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapAppear : Trap
{
    private void Start()
    {
        Disappear();
    }
    public override void ActivateSkill()
    {
        Debug.Log($"Trap {name} activates its skill: disappear");
        Reappear();
    }

    public override void DeactivateSkill()
    {
        Debug.Log($"Trap {name} deactivates its skill: reappear");
        Disappear();
    }
}
