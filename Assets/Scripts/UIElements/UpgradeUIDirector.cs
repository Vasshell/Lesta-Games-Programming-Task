using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

public class UpgradeUIDirector : MonoBehaviour
{
    [SerializeField] public GameObject buttonPrefab;
    [SerializeField] public GameObject labelPrefab;
    [SerializeField] private Vector2 _topButtonCoords;
    [SerializeField] private Vector2 _middleButtonCoords;
    [SerializeField] private Vector2 _bottomButtonCoords; 
    [SerializeField] private Vector2 _weaponButtonCoords;
    [SerializeField] private Vector2 _statTextCoords;
    private Dictionary<Vector2, UIButton> _buttons = new Dictionary<Vector2, UIButton>();
    private UIButton _weaponButton;
    private Canvas _canvas;
    private UIText _statText;

    private void Start()
    {
        SetFreeButtonsCoords();
        _canvas = GetComponentInParent<Canvas>();
        _statText = Instantiate(labelPrefab, _canvas.transform).GetComponent<UIText>();
    }

    private void SetFreeButtonsCoords()
    {
        _buttons.Add(_topButtonCoords, null);
        _buttons.Add(_middleButtonCoords, null);
        _buttons.Add(_bottomButtonCoords, null);
    }

    public void DisplayStatNumbers((int strength, int agility, int stamina) stats)
    {
        _statText.AssignText($"Сила: {stats.strength} Ловкость: {stats.agility} Выносливость: {stats.stamina}");
    }

    public void DisplayButton(UnityAction action, string text)
    {
        foreach (var button in _buttons)
        {
            if (button.Value == null)
            {
                _buttons[button.Key] = CreateNewButton(button.Key, text, action);
                return;
            } 
        }
        throw new Exception("More buttons than expected");
    }

    private UIButton CreateNewButton(Vector2 coords, string text, UnityAction action)
    {
        GameObject gameObject = Instantiate(buttonPrefab, _canvas.transform);
        UIButton uIButton = gameObject.GetComponent<UIButton>();
        uIButton.AssignCoorditates(coords);
        uIButton.AssignText(text);
        uIButton.AssignDelegate(action);
        return uIButton;
    }

    internal void RemoveButtons()
    {
        List<UIButton> killedButtons = new List<UIButton>();
        foreach(var button in _buttons)
        {
            var killbutton = button.Value;
            killedButtons.Add(killbutton);
        }
        _buttons.Clear();
        SetFreeButtonsCoords();
        foreach (var killbutton in killedButtons)
        {
            killbutton.DestroyButton();
        }
    }
}
