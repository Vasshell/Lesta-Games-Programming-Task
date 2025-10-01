using TMPro;
using UnityEngine;

public class UIText : MonoBehaviour
{
    private TextMeshProUGUI _text;

    void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void AssignText(string text)
    {
        _text.text = text;
    }

}
