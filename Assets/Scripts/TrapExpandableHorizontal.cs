using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExpandableHorizontal : Trap
{
    public float expand = 2f;

    public override void ActivateSkill()
    {
        Debug.Log($"Trap {name} activates its skill: expand horizontally");
        ExpandHorizontally(expand);
    }

    public override void DeactivateSkill()
    {
        Debug.Log($"Trap {name} deactivates its skill: collapse to original size");
        Collapse();
    }

}
