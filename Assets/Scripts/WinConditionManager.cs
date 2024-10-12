using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinConditionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private int playersInZone = 0; // Counter for players in the win zone

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if a player has entered the win zone
        if (other.CompareTag("Player"))
        {
            playersInZone++;

            Debug.Log("Player Enter");

            Debug.Log(playersInZone);


            // If both players are in the win zone, change to the win screen
            if (playersInZone == 1)
            {
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
