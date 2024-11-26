using UnityEngine;

public class AspectRatioAdjust : MonoBehaviour
{
    private new Camera camera;
    private readonly float defaultAspectRatio = 16.0f / 9.0f;
    private float prevAspect = 0;

    void Start()
    {
        camera = GetComponent<Camera>();
        AdjustAspectRatio();
    }

    void Update()
    {
        AdjustAspectRatio();
    }

    private void AdjustAspectRatio()
    {
        if (prevAspect == camera.aspect) return;

        prevAspect = camera.aspect;

        if (defaultAspectRatio == camera.aspect || defaultAspectRatio < camera.aspect)
        {
            if (camera.orthographicSize == 18) return;
            camera.orthographicSize = 18;
        }
        else camera.orthographicSize = 18 / (camera.aspect / defaultAspectRatio);
    }
}
