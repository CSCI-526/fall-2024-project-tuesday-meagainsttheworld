using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DataCollection
{
    public readonly static int buildNo = 5;
    public static string SessionID { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeInitialized()
    {
        SessionID = DateTime.Now.Ticks.ToString();
        Debug.Log("Session ID: " + SessionID);
    }

    public static async void Post(string URL, WWWForm form)
    {
        // Prevent data from being sent while testing in Unity editor
        if (Application.isEditor) return;

        // Send responses and verify result
        UnityWebRequest www = UnityWebRequest.Post(URL, form);

        Debug.Log("Sending Data");

        www.SendWebRequest();

        bool timedOut = false;
        float timePassed = 0;

        while (www.result == UnityWebRequest.Result.InProgress)
        {
            if (timePassed > 10)
            {
                timedOut = true;
                break;
            }
            timePassed += 0.1f;
            await Task.Delay(100);
        }

        if (timedOut) Debug.Log("Web Request took longer than 10 seconds process.");
        else
        {
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
            Debug.Log("Request took roughly " + timePassed + " seconds to process.");
        }
    }
}
