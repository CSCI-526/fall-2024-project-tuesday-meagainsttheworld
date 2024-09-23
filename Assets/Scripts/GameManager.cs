using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI TrapsText;
    public TextMeshProUGUI WinText;
    private PlayerHealth playerHealth;
    public bool isGameOver;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerHealth.life == 0){
            lifeText.text = "World Win!";
            WinText.gameObject.SetActive(true);
        }
        /*else if(     if player reach the flag/endpoint        ){
            lifeText.text = "Me Win!";


        }*/
    }

    public void StartGame(){
        titleScreen.gameObject.SetActive(false);
        lifeText.text = "Lives: " + playerHealth.life;
        //also need traps number here !
    }

}
