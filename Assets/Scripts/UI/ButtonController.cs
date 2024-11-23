using System;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject wall;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float posDiff = Math.Abs(startPos.y - transform.localPosition.y);
        if (posDiff > 0.95) transform.localPosition = Vector3.zero;
        if (posDiff > 0.2) wall.SetActive(false);
        else wall.SetActive(true);
    }
}
