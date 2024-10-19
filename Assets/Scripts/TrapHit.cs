using UnityEngine;
using UnityEngine.SceneManagement;

public class TrapHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trap"))
        {
            //New Code by Ziang Qin: only for alpha Analytics
            WinConditionManager.entryCount++;
            string currentSceneName = SceneManager.GetActiveScene().name;
            Vector2 playerPosition = transform.position;
            Debug.Log("Death Scene: " + currentSceneName + ", Death Player Position: " + playerPosition + ", Deaths Count: " + WinConditionManager.entryCount);
            // Change the scene to the Game Over screen
            SceneSwitcher.lastLevel = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("GameOver");
        }
    }
}
