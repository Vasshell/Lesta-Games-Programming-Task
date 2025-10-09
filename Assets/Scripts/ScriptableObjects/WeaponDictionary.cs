using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDictionary", menuName = "Scriptable Objects/WeaponDictionary")]
public class WeaponDictionary : ScriptableObject
{
    [SerializeField] private List<Weapon> weapons;

    public Weapon this[string name] => weapons.Find(weapon => weapon.idName == name);
}
