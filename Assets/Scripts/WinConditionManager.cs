using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    private static int playersInZone = 0; // Counter for players in the win zone

    void Start()
    {
        playersInZone = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a player has entered the win zone
        if (other.CompareTag("Player"))
        {
            playersInZone++;

            GetComponent<SpriteRenderer>().color += new Color(0,0,0,0.5f);

            Debug.Log("Player Enter");

            Debug.Log(playersInZone);

            // If both players are in the win zone, change to the win screen
            if (playersInZone == 2) StartCoroutine(WaitForWinScreen());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if a player has exited the win zone
        if (other.CompareTag("Player"))
        {
            GetComponent<SpriteRenderer>().color -= new Color(0,0,0,0.5f);

            Debug.Log("Player Exit");
            playersInZone--;
        }
    }

    private IEnumerator WaitForWinScreen()
    {
        yield return new WaitForSeconds(0.2f);

        SceneSwitcher.prevLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("YouWin");
    }
}
