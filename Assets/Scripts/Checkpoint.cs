using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private int checkpointNum;
    [SerializeField] private Vector2 p1GroundVector = Vector2.down;
    private Vector2 p1Point;
    private Vector2 p2Point;
    private bool p1Reached;
    private bool p2Reached;
    
    void Start()
    {
        p1Point = transform.GetChild(0).transform.position;
        p2Point = transform.GetChild(1).transform.position;
        int.TryParse(Regex.Match(name, @"\d+").Value, out checkpointNum);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (SpawnpointManager.lastCheckpointNum < checkpointNum && !(p1Reached && p2Reached) && other.CompareTag("Player"))
        {
            if (!p1Reached && other.name.Equals("Player1")) p1Reached = true;
            else if (!p2Reached && other.name.Equals("Player2")) p2Reached = true;
            
            if (p1Reached && p2Reached)
            {
                Debug.Log("Both players reached checkpoint");

                SpawnpointManager.playerSpawnStates[0].currSpawnPoint = p1Point;
                SpawnpointManager.playerSpawnStates[0].currGroundVector = p1GroundVector;
                SpawnpointManager.playerSpawnStates[1].currSpawnPoint = p2Point;
                SpawnpointManager.playerSpawnStates[1].currGroundVector = -p1GroundVector;

                SpawnpointManager.lastCheckpointNum = checkpointNum;
                SetCheckpointSpriteActive();
            }
        }
    }

    public void SetCheckpointSpriteActive()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
    }
}
