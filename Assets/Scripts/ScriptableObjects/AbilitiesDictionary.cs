using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Scriptable Objects/AbilitiesDictionary")]
public class AbilitiesDictionary : ScriptableObject
{

    public List<Ability> abilities;

    public Ability this[string name] => abilities.Find(ability => ability.idName == name);



}
