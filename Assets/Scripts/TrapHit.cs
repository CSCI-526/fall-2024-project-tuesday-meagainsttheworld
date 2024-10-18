using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapHit : MonoBehaviour
{
    //New Code by Ziang Qin: only for alpha Analytics
    private int entryCount = 0;
    private WinConditionManager winConditionManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            //New Code by Ziang Qin: only for alpha Analytics
            entryCount++;
            string currentSceneName = SceneManager.GetActiveScene().name;
            Vector2 playerPosition = transform.position;
            Debug.Log("Death Scene: " + currentSceneName + ", Death Player Position: " + playerPosition + ", Deaths Count: " + entryCount);
            winConditionManager.SetEntryCount(entryCount);
            // Change the scene to the Game Over screen
            SceneManager.LoadScene("GameOver");
        }
    }
}
