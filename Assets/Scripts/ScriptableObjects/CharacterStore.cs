using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStore", menuName = "Scriptable Objects/Character")]
public class CharacterStore : ScriptableObject
{
    [SerializeField] public int attStrength;
    [SerializeField] public int attAgility;
    [SerializeField] public int attStamina;
    [SerializeField] public int level = 0;
    [SerializeField] public int health;
    [SerializeField] public Weapon weapon;
    [SerializeField] public int dummyWeapon;
    [SerializeField] public List<Ability> abilities = new List<Ability>();


    public void SetCharacter(Character character)
    {
        attStrength = character.GetStrength();
        attAgility = character.GetAgility();
        attStamina = character.GetStamina();
        level = character.GetLevel();
        health = character.GetHealth();
        abilities = character.GetAbilities();
        weapon = character.GetWeapon();
        dummyWeapon = character.GetDummyWeapon();
    }
}
