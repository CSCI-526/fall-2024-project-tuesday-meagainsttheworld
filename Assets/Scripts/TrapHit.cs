using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapHit : MonoBehaviour
{
    private readonly string deathCountURL = "https://docs.google.com/forms/d/e/1FAIpQLSd0XcbT8jZLLgnJI22RhSL7xweuyTPJfPwONQbg0C2zk805zA/formResponse";

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
            StartCoroutine(PlayDeath());
        }
    }

    public IEnumerator PlayDeath()
    {
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<TrailRenderer>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).GetComponent<ParticleSystem>().Play();

        yield return new WaitForSeconds(0.5f);

        SceneSwitcher.lastLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        // Current build name
        form.AddField("entry.1014429915", 1);

        DataCollection.Post(deathCountURL, form);
    }
}
