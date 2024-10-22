using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapHit : MonoBehaviour
{
    private readonly string deathCountURL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScErtwXtsd4chv4uG1cORolwI5USv1hmsTOUjnRWbbJHcGgrg/formResponse";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            //New Code by Ziang Qin: only for alpha Analytics
            // WinConditionManager.entryCount++;
            // string currentSceneName = SceneManager.GetActiveScene().name;
            // Vector2 playerPosition = transform.position;
            // Debug.Log("Death Scene: " + currentSceneName + ", Death Player Position: " + playerPosition + ", Deaths Count: " + WinConditionManager.entryCount);
            
            SendDeath();
            SceneSwitcher.lastLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameOver");
        }
    }

    public void SendDeath()
    {
        // Create the form and enter responses
        WWWForm form = new();
        
        // Session ID
        form.AddField("entry.1554864013", DataCollection.sessionID);
        // Player ID
        form.AddField("entry.298539410", gameObject.name);
        // Scene Name
        form.AddField("entry.1159607511", SceneManager.GetActiveScene().name);
        // Player X coordinate at time of death
        form.AddField("entry.1621254126", transform.position.x.ToString());
        // Player Y coordinate at time of death
        form.AddField("entry.1068866632", transform.position.y.ToString());

        DataCollection.Post(deathCountURL, form);
    }
}
