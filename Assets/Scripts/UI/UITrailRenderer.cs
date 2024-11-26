using UnityEngine;
using UnityEngine.UI;

public class UITrailRenderer : MonoBehaviour
{
    public GameObject trailObject;
    private RectTransform rectTransform;
    private RectTransform canvasRectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasRectTransform = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    void Update()
    {
        Vector2 anchoredPosition = rectTransform.anchoredPosition;
        Vector2 worldPosition = CalculateWorldPosition(anchoredPosition);
        trailObject.transform.position = worldPosition;
    }

    private Vector2 CalculateWorldPosition(Vector2 anchoredPosition)
    {
        Vector2 normalizedPosition = new Vector2(
            (anchoredPosition.x / canvasRectTransform.sizeDelta.x) + 0.5f,
            (anchoredPosition.y / canvasRectTransform.sizeDelta.y) + 0.5f
        );
        return new Vector2(
            normalizedPosition.x * Screen.width,
            normalizedPosition.y * Screen.height
        );
    }
}