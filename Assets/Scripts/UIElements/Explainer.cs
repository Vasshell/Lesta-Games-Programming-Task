using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Explainer : MonoBehaviour
{
    private string _text;
    private UIText _uiText;
    private bool _hasText;
    [SerializeField] private GameObject _uiTextPrefab;

    public void SetText(string text)
    {
        _text = text;
        _hasText = true;
    }

    public void OnPointerEnter()
    {
        if (_hasText)
        {
            _uiText = Instantiate(_uiTextPrefab, this.gameObject.transform).GetComponent<UIText>();
            _uiText.gameObject.transform.position = Mouse.current.position.value;
            _uiText.gameObject.transform.Translate(new Vector2(-1 * _uiText.GetPreferredWidth(), 0));
            _uiText.SetFontSize(26);
            _uiText.AssignText(_text);
        }
    }

    public void OnPointerExit()
    {
        if (_hasText)
        {
            Destroy(_uiText.gameObject);
        }
    }
}
