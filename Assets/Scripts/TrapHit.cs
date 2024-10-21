using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class TrapHit : MonoBehaviour
{


    private readonly string URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScErtwXtsd4chv4uG1cORolwI5USv1hmsTOUjnRWbbJHcGgrg/formResponse";

    private string playerDead;
    private string currLevel;
    private float playerX;
    private float playerY;

    public void Send()
    {

        // Assign variables
        playerDead = gameObject.name;
        currLevel = SceneManager.GetActiveScene().name;
        playerX = transform.position.x;
        playerY = transform.position.y;
        StartCoroutine(Post(playerDead.ToString(), currLevel.ToString(), playerX.ToString(), playerY.ToString()));
    }

    private IEnumerator Post(string playerDead, string currLevel, string playerX, string playerY)
    {
        // Create the form and enter responses
        WWWForm form = new();
        form.AddField("entry.298539410", playerDead);
        form.AddField("entry.1159607511", currLevel);
        form.AddField("entry.1621254126", playerX);
        form.AddField("entry.1068866632", playerY);

        // Send responses and verify result
        using UnityWebRequest www = UnityWebRequest.Post(URL, form);

        Debug.Log("Sending Data");

        yield return www.SendWebRequest();

        Debug.Log("Data Sent");

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }

        SceneSwitcher.lastLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("GameOver");
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            //New Code by Ziang Qin: only for alpha Analytics
            // WinConditionManager.entryCount++;
            // string currentSceneName = SceneManager.GetActiveScene().name;
            // Vector2 playerPosition = transform.position;
            // Debug.Log("Death Scene: " + currentSceneName + ", Death Player Position: " + playerPosition + ", Deaths Count: " + WinConditionManager.entryCount);
            
            Send();
        }
    }
}
