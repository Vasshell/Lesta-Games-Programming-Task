using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButton : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Button _button;
    private Image _image;
    private RectTransform _rectTransform;

    void Awake()
    {   
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TextMeshProUGUI>();                   
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
    }

    public void AssignCoorditates(Vector2 coords)
    {
        _rectTransform.anchoredPosition = coords;
    }

    public void AssignText(string text)
    {
        _text.text = text;
    }

    public void AssignImage(Image image)
    {
        _image = image;
    }

    public void AssignDelegate(UnityAction unityAction)
    {
        _button.onClick.AddListener(unityAction);
    }

    public Vector2 GetCoordinates() => _rectTransform.anchoredPosition;
    public void DestroyButton() => Destroy(this.gameObject);
}
