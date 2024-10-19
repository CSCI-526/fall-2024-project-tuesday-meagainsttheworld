using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    private int playersInZone = 0; // Counter for players in the win zone
    //New Code from Ziang Qin: only for alpha Analytics
    public static int entryCount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a player has entered the win zone
        if (other.CompareTag("Player"))
        {
            playersInZone++;

            Debug.Log("Player Enter");

            Debug.Log(playersInZone);


            // If both players are in the win zone, change to the win screen
            if (playersInZone == 2)
            {
                //New Code from Ziang Qin: only for alpha Analytic"s
                Debug.Log("Total Death Count for this run of " + SceneManager.GetActiveScene().name + ": " + entryCount);
                entryCount = 0;
                SceneSwitcher.lastLevel = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene("YouWin");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if a player has exited the win zone
        if (other.CompareTag("Player"))
        {

            Debug.Log("Player Exit");
            playersInZone--;
        }
    }
}
