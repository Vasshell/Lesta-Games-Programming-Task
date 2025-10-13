using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private RectTransform _txtRectTransform;
    private RectTransform _bgRectTransform;

    void Awake()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _txtRectTransform = GetComponentInChildren<RectTransform>();
        _bgRectTransform = GetComponent<RectTransform>();
    }
    
    public float GetPreferredWidth() => _text.preferredWidth;

    public void SetFontSize(int size) => _text.fontSize = size;

    public void AssignText(string text)
    {
        _text.text = text;
        _txtRectTransform.sizeDelta = new Vector2(_text.preferredWidth, _text.preferredHeight);
        _bgRectTransform.sizeDelta = new Vector2(_text.preferredWidth, _text.preferredHeight);
    }

    public void AssignCoordinates(Vector2 coords)
    {
        _txtRectTransform.anchoredPosition = coords;
        _bgRectTransform.anchoredPosition = coords;
    }
}
