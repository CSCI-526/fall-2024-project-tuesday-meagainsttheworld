using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Transform respawnPoint;
    private Rigidbody2D playerRb;
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
        
        Debug.Log("Life -1");

        // back to spawn point
        transform.position = respawnPoint.position;

        playerRb.velocity = Vector2.zero;

    }
}
