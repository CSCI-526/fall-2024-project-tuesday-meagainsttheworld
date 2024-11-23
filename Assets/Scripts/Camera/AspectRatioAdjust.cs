using UnityEngine;

public class AspectRatioAdjust : MonoBehaviour
{
    private new Camera camera;
    private readonly float defaultAspectRatio = 16.0f / 9.0f;

    // Start is called before the first frame update
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
        if (defaultAspectRatio == camera.aspect || defaultAspectRatio < camera.aspect)
        {
            if (camera.orthographicSize == 18) return;
            camera.orthographicSize = 18;
        }
        else camera.orthographicSize = 18 / (camera.aspect / defaultAspectRatio);
    }
}
