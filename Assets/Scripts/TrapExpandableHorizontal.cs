using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExpandableHorizontal : Trap
{

    public override void ActivateSkill()
    {
        Debug.Log($"Trap {name} activates its skill: expand horizontally");
        ExpandHorizontally();
    }

    public override void DeactivateSkill()
    {
        Debug.Log($"Trap {name} deactivates its skill: collapse to original size");
        Collapse();
    }

}
