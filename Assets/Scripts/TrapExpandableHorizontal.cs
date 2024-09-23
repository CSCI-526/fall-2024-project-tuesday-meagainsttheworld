using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapExpandableHorizontal : Trap
{
    // Start is called before the first frame update
    void Start()
    {
        isControllable = true;
        isDangerous = true;
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    public override void UseSkill()
    {
        Debug.Log($"Trap {name} uses its skill: expand horizontally");
        // transform.

    }
    public override void Move(Vector2 direction)
    {
        Vector2 dirX = new Vector2(direction.x, 0);
        transform.Translate(dirX * Time.deltaTime * moveSpeed);
    }

}
