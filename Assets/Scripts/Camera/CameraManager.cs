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

        Vector3 camStartPos = (SpawnpointManager.playerSpawnStates[1].currSpawnPoint + SpawnpointManager.playerSpawnStates[0].currSpawnPoint) / 2;
        camStartPos += new Vector3(0, 0, -10);

        foreach (CinemachineVirtualCamera cam in camList)
        {
            if (cam.Follow != null) cam.ForceCameraPosition(camStartPos, Quaternion.identity);
        }

        Camera mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        mainCam.transform.position = camList[currCam].transform.position;
        mainCam.orthographicSize = camList[currCam].m_Lens.OrthographicSize;
        mainCam.transform.position = camList[currCam].transform.position;

        LoadCam(currCam);
    }

    public void LoadCam(int camNum)
    {
        currCam = camNum;
        Debug.Log("Loading Cam (" + camNum + ")");
        camNum--;
        camList[camNum].gameObject.SetActive(false);
        camList[camNum].gameObject.SetActive(true);
    }
}
