using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnpointManager : MonoBehaviour
{
    [System.Serializable]
    public class PlayerSpawnState
    {
        public PlayerSpawnState(PlayerController inputPlayer)
        {
            origSpawnPoint = inputPlayer.transform.position;
            origGroundVector = inputPlayer.groundVector;
            currSpawnPoint = origSpawnPoint;
            currGroundVector = origGroundVector;
        }

        public void ChangeSpawn(Vector2 newSpawnPoint, Vector2 newGroundVector)
        {
            this.currSpawnPoint = newSpawnPoint;
            this.currGroundVector = newGroundVector;
        }

        public Vector2 origSpawnPoint;
        public Vector2 origGroundVector;
        public Vector2 currSpawnPoint;
        public Vector2 currGroundVector;
    }

    public static List<PlayerSpawnState> playerSpawnStates = new();
    public static int lastCheckpointNum = 0;

    void Awake()
    {
        if (playerSpawnStates.Count == 0)
        {
            PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
            players = players.OrderBy(player => player.name).ToArray();
            playerSpawnStates.Add(new PlayerSpawnState(players[0]));
            playerSpawnStates.Add(new PlayerSpawnState(players[1]));
        }
        
        if (lastCheckpointNum != 0)
        {
            transform.GetChild(lastCheckpointNum - 1).GetComponent<Checkpoint>().SetCheckpointSpriteActive();
            RespawnPlayers();
        }
    }

    private void RespawnPlayers()
    {
        PlayerController[] players = GameObject.FindObjectsOfType<PlayerController>();
        players = players.OrderBy(player => player.name).ToArray();

        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("Spawning " + players[i].name + " at " + playerSpawnStates[i].currSpawnPoint + " with ground vector " + playerSpawnStates[i].currGroundVector);
            players[i].transform.position = playerSpawnStates[i].currSpawnPoint;
            players[i].GetComponent<Rigidbody2D>().gravityScale = -playerSpawnStates[i].currGroundVector.y;
        }
    }
}
