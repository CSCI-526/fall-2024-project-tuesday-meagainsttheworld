using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Trap : MonoBehaviour
{
    // properties
    public bool isDangerous = true;
    public bool isControllable = false;

    public float moveSpeed = 10.0f;
    public abstract void UseSkill();
    public abstract void Move(Vector2 direction);


}
