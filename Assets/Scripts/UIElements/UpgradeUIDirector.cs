using NUnit.Framework;
using System;
using System.Collections;
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
    [SerializeField] private UIPositionMarker[] _buttonCoords;
    [SerializeField] private UIPositionMarker _weaponButtonCoords;
    [SerializeField] private UIPositionMarker _statTextCoords;
    private Dictionary<Vector2, UIButton> _buttons = new Dictionary<Vector2, UIButton>();
    private UIButton _weaponButton;
    private Canvas _canvas;
    private UIText _statText;

    public IEnumerator Begin()
    {
        _canvas = GetComponentInParent<Canvas>();
        _statText = Instantiate(labelPrefab, _canvas.transform).GetComponent<UIText>();
        _statText.AssignCoordinates(_statTextCoords.GetPosition());
        SetFreeButtonsCoords();
        yield break;
    }

    private void SetFreeButtonsCoords()
    {
        foreach (var button in _buttonCoords)
        {
            _buttons.Add(button.GetPosition(), null);
        }
    }

    public void DisplayStatNumbers((int strength, int agility, int stamina) stats)
    {
        _statText.AssignText($"Сила: {stats.strength}\nЛовкость: {stats.agility}\nВыносливость: {stats.stamina}");
    }

    public void DisplayButton(UnityAction action, string text, string description = null)
    {
        foreach (var button in _buttons)
        {
            if (button.Value == null)
            {
                _buttons[button.Key] = CreateNewButton(button.Key, text, action, description);
                return;
            } 
        }
        throw new Exception("More buttons than expected");
    }

    private string DamageTypeToText(DamageType damageType)
    {
        string damageName = "Урон";
        switch (damageType)
        {
            case DamageType.Blunt:
                damageName = "Дробящий урон";
                break;
            case DamageType.Slash:
                damageName = "Рубящий урон";
                break;
            case DamageType.Stab:
                damageName = "Колющий урон";
                break;
            default:
                break;
        }
        return damageName;
    }

    public void DisplayWeaponButton(Weapon weapon, UnityAction action, Weapon oldweapon = null)
    {
        _weaponButton = CreateNewButton(_weaponButtonCoords.GetPosition(), weapon.GetWpnName(), action);
        _weaponButton.AssignImage(weapon.GetImage());
        int damage = weapon.GetDamage();
        DamageType damageType = weapon.GetDamageType();
        var newdamageName = DamageTypeToText(weapon.GetDamageType());  
        if (oldweapon != null)
        {
            var olddamageName = DamageTypeToText(oldweapon.GetDamageType());
            _weaponButton.AssignDescription($"{newdamageName}: {damage}\nСейчас: {oldweapon.GetWpnName()} ({olddamageName}: {oldweapon.GetDamage()})");
        }
        else
        {
            _weaponButton.AssignDescription($"{newdamageName}: {damage}");
        }
    }

    public void RemoveWeaponButton()
    {
        _weaponButton.DestroyButton();
    }

    private UIButton CreateNewButton(Vector2 coords, string text, UnityAction action, string description = null)
    {
        GameObject gameObject = Instantiate(buttonPrefab, _canvas.transform);
        UIButton uIButton = gameObject.GetComponent<UIButton>();
        uIButton.AssignCoorditates(coords);
        uIButton.AssignText(text);
        uIButton.AssignDelegate(action);
        if (description != null) uIButton.AssignDescription(description);
        return uIButton;
    }

    internal void RemoveButtons()
    {
        List<UIButton> killedButtons = new List<UIButton>();
        foreach(var button in _buttons)
        {
            if (button.Value != null)
            {
                var killbutton = button.Value;
                killedButtons.Add(killbutton);
            }
        }
        _buttons.Clear();
        SetFreeButtonsCoords();
        foreach (var killbutton in killedButtons)
        {
            killbutton.DestroyButton();
        }
    }
}
