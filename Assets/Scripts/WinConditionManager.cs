using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    private int playersInZone = 0; // Counter for players in the win zone
    //New Code from Ziang Qin: only for alpha Analytics
    // public static int entryCount;

    //New Code by Ziang Qin in Beta Phase
    public GameManager gameManager; // Reference to the GameManager

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
                //New Code from Ziang Qin: only for beta Analytics
                Debug.Log($"Player 1 Wait Time: {gameManager.WaitTimePlayer1}");
                Debug.Log($"Player 2 Wait Time: {gameManager.WaitTimePlayer2}");

                StartCoroutine(WaitForWinScreen());
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

    private IEnumerator WaitForWinScreen()
    {
        yield return new WaitForSeconds(0.2f);

        SceneSwitcher.prevLevel = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("YouWin");
    }
}
