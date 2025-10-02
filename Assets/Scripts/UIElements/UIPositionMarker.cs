using UnityEngine;

public class UIPositionMarker: MonoBehaviour
{
    private RectTransform _rectTransform;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public Vector2 GetPosition() => _rectTransform.anchoredPosition;
}
