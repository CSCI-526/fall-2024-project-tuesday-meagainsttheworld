using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] camList = null;
    [field: SerializeField] public bool TransitionCamPresent { get; private set; } = false;
    public static int currCam = 1;

    [ContextMenu("Generate Camera List")]
    private void GenerateCamList()
    {
        camList = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        camList = camList.OrderBy(cam => cam.name).ToArray();
    }

    void Start()
    {
        if (camList == null) GenerateCamList();

        Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCam.transform.position = (SpawnpointManager.playerSpawnStates[1].currSpawnPoint + SpawnpointManager.playerSpawnStates[0].currSpawnPoint) / 2;
        mainCam.orthographicSize = camList[SpawnpointManager.lastCheckpointNum].m_Lens.OrthographicSize;
        mainCam.transform.position = camList[SpawnpointManager.lastCheckpointNum].transform.position;
        foreach (CinemachineVirtualCamera cam in camList)
        {
            cam.ForceCameraPosition((SpawnpointManager.playerSpawnStates[1].currSpawnPoint + SpawnpointManager.playerSpawnStates[0].currSpawnPoint) / 2, Quaternion.identity);
        }

        LoadCam(currCam);
    }

    public void LoadCam(int camNum)
    {
        currCam = camNum;
        camNum--;
        Debug.Log("Loading Cam (" + camNum + ")");
        camList[camNum].gameObject.SetActive(false);
        camList[camNum].gameObject.SetActive(true);
    }
}
