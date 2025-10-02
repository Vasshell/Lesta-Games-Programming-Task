using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private RectTransform _rectTransform;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void AssignText(string text)
    {
        _text.text = text;
    }

    public void AssignCoordinates(Vector2 coords)=>_rectTransform.anchoredPosition = coords;
}
