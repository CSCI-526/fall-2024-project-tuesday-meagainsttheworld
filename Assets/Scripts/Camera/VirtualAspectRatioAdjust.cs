using Cinemachine;
using UnityEngine;

public class VirtualAspectRatioAdjust : MonoBehaviour
{
    private Camera mainCamera;
    private new CinemachineVirtualCamera camera;
    private readonly float defaultAspectRatio = 16.0f / 9.0f;
    private float prevAspect = 0;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera = GetComponent<CinemachineVirtualCamera>();
        AdjustAspectRatio();
    }

    void Update()
    {
        AdjustAspectRatio();
    }

    private void AdjustAspectRatio()
    {
        if (prevAspect == mainCamera.aspect) return;

        prevAspect = mainCamera.aspect;

        if (defaultAspectRatio == mainCamera.aspect || defaultAspectRatio < mainCamera.aspect)
        {
            if (camera.m_Lens.OrthographicSize == 18) return;
            camera.m_Lens.OrthographicSize = 18;
        }
        else camera.m_Lens.OrthographicSize = 18 / (mainCamera.aspect / defaultAspectRatio);
    }
}
