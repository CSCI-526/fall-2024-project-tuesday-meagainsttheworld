using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExpandableHorizontal : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        // isControllable = true;
        // isDangerous = true;
        moveHorizontally = true;
        originalScale = transform.localScale;
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public override void ActivateSkill()
    {
        Debug.Log($"Trap {name} activates its skill: expand horizontally");
        ExpandHorizontally();
    }

    public override void DeactivateSkill()
    {
        Debug.Log($"Trap {name} deactivates its skill: expand horizontally");
        Collapse();
    }

}
