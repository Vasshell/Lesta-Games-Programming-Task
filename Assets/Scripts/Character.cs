using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    private int _attStrength;
    private int _attAgility;
    private int _attStamina;
    private int _clRogue = 0;
    private int _clWarrior = 0;
    private int _clBerserker = 0;
    private int _level = 0;
    private List<Ability> _abilities;

    public void NewPlayerCharacter()
    {
        _attStrength = Random.Range(1, 3);
        _attAgility = Random.Range(1, 3);
        _attStamina = Random.Range(1, 3);
    }

    public (int strength, int agility, int stamina) GetStats()
    {
        return (_attStrength,_attAgility,_attStamina);
    }

    public (int rogue, int warrior, int berserker) GetLevels()
    {
        return (_clRogue, _clWarrior, _clBerserker);
    }

    public void AddAbility(Ability ability)
    {
        _abilities.Add(ability);
    }

    public void RemoveAbility(Ability ability)
    {
        _abilities.Remove(ability);
    }

    public void SetWarriorLevel(int level) => _clWarrior = level;
    public void SetRogueLevel(int level) => _clRogue=level;
    public void SetBerserkerLevel(int level) => _clBerserker=level;
    public void LevelUp() => _level += 1;
    public int GetLevel() => _level;
}
