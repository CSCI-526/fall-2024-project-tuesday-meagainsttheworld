using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetGroupSet : MonoBehaviour
{
    void Awake()
    {
        if (SpawnpointManager.lastCheckpointNum != 0)
        {
            transform.position = (SpawnpointManager.playerSpawnStates[1].currSpawnPoint + SpawnpointManager.playerSpawnStates[0].currSpawnPoint) / 2;
        }
    }
}
