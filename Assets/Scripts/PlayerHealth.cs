using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public GameObject corpsePrefab;
    public Transform respawnPoint;
    private Rigidbody2D playerRb;
    public GameManager gameManager;
    public int life;
    public TextMeshProUGUI lifeText;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Traps"))
        {
            Die();
        }
    }

    private void Die()
    {
        if (gameManager.isWorldWinner || life <= 0)
        {
            return;
        }
        // get a dead body
        Quaternion corpseRotation = Quaternion.Euler(0, 0, 90);
        Instantiate(corpsePrefab, transform.position + new Vector3(1f, 0, 0), corpseRotation);
        Debug.Log("Corpse Rotation: " + corpsePrefab.transform.rotation.eulerAngles);

        // back to spawn point
        transform.position = respawnPoint.position;

        playerRb.velocity = Vector2.zero;

        gameManager.OnPlayerDeath();  // This reduces life in GameManager
    }
}