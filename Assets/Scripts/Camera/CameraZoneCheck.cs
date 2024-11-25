using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CameraZoneCheck : MonoBehaviour
{
    private int camZoneNum;
    private bool p1Present;
    private bool p2Present;
    private CameraManager camManager;

    void Start()
    {
        camManager = transform.parent.GetComponent<CameraManager>();
        int.TryParse(Regex.Match(name, @"\d+").Value, out camZoneNum);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!(p1Present && p2Present) && other.CompareTag("Player"))
        {
            if (!p1Present && other.name.Equals("Player1")) p1Present = true;
            else if (!p2Present && other.name.Equals("Player2")) p2Present = true;

            if (p1Present && p2Present)
            {
                if (camManager.TransitionCamPresent || CameraManager.currCam < camZoneNum)
                {
                    transform.parent.GetComponent<CameraManager>().LoadCam(camZoneNum);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!p1Present && other.name.Equals("Player1")) p1Present = false;
        else if (!p2Present && other.name.Equals("Player2")) p2Present = false;

        if (p1Present != p2Present && camManager.TransitionCamPresent)
        {
            transform.parent.GetComponent<CameraManager>().LoadCam(1);
        }
    }
}
