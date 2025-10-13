using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{
    [SerializeField] private string _charName;
    [SerializeField] private int _attStrength;
    [SerializeField] private int _attAgility;
    [SerializeField] private int _attStamina;
    [SerializeField] private int _level = 0;
    [SerializeField] private int _health;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private int _dummyWeapon = 0;
    [SerializeField] private List<Ability> _abilities = new List<Ability>();

    public void LoadCharacter(CharacterStore character)
    {
        _charName = character.charName;
        _attStrength = character.attStrength;
        _attAgility = character.attAgility;
        _attStamina = character.attStamina;
        _level = character.level;
        _abilities = character.abilities;
        _health = character.health;
        _weapon = character.weapon;
        _dummyWeapon = character.dummyWeapon;
    }

    public void NewPlayerCharacter()
    {
        _attStrength = Random.Range(1, 3);
        _attAgility = Random.Range(1, 3);
        _attStamina = Random.Range(1, 3);
    }

    public int GetStrength() => _attStrength;
    public void SetStrength(int newStrength) => _attStrength = newStrength;

    public int GetAgility() => _attAgility;
    public void SetAgility(int newAgility) => _attAgility = newAgility;

    public int GetStamina() => _attStamina;
    public void SetStamina(int newStamina) => _attStamina = newStamina;

    public List<Ability> GetAbilities() => _abilities;
    public void SetAbilities(List<Ability> abilities) => _abilities = abilities;
    public int GetHealth() => _health;
    public void SetHealth(int health) => _health = health;
    public Weapon GetWeapon() => _weapon;
    public void SetWeapon(Weapon weapon) => _weapon = weapon;
    public int GetDummyWeapon() => _dummyWeapon;
    public void SetDummyWeapon(int dmg) => _dummyWeapon = dmg;

    public (int strength, int agility, int stamina) GetStats()
    {
        return (_attStrength, _attAgility, _attStamina);
    }

    public bool HasAbility(string abilityName) => _abilities.Contains(FindAbility(abilityName));

    public void AddAbility(Ability ability) => _abilities.Add(ability);

    public void ApplyPermanentAbility(Ability ability)
    {
        AddAbility(ability);
        ability.StatAbilityCall(this);
    }

    public void RemoveAbility(Ability ability) => _abilities.Remove(ability);

    public Ability FindAbility(string abilityName) => _abilities.Find(ability => ability.idName == abilityName);

    public string FindNextAbility(string abilityName)
    {
        if (HasAbility(abilityName))
        {
            return FindNextAbility(FindAbility(abilityName).nextAbility);
        }
        else return abilityName;
    }

    public List<Ability> FindAllAbilitiesOfType(AbilityType abilityType) => _abilities.FindAll(ability => ability.type == abilityType);

    public void LevelUp()
    {
        _level += 1;
        _health += _attStamina;
    }

    public void SetLevel(int level) => _level = level;
    public int GetLevel() => _level;

    public void TakeDamage(Damage damage) => _health -= damage.TallyUpDamage();

    public Damage DealDamage()
    {
        if (_dummyWeapon != 0) return new Damage(_attStrength, _dummyWeapon, _weapon.GetDamageType(), 0);
        return new Damage(_attStrength, _weapon.GetDamage(), _weapon.GetDamageType(), 0);
    }

    public string GetName() => _charName;
}

public class Damage
{
    public int dmgBase;
    public int dmgWeapon;
    public DamageType damageType;
    public int dmgPerk;

    public Damage(int dmgBase, int dmgWeapon, DamageType damageType, int dmgPerk)
    {
        this.dmgBase = dmgBase;
        this.dmgWeapon = dmgWeapon;
        this.damageType = damageType;
        this.dmgPerk = dmgPerk;
    }

    public Damage(Damage damage)
    {
        this.dmgBase = damage.dmgBase;
        this.dmgWeapon = damage.dmgWeapon;
        this.damageType = damage.damageType;
        this.dmgPerk = damage.dmgPerk;
    }

    public bool Compare(Damage damage)
    {
        bool result = false;
        if (dmgBase != damage.dmgBase) result = true;
        if (dmgWeapon != damage.dmgWeapon) result = true;
        if(damageType != damage.damageType) result = true;
        if (dmgPerk != damage.dmgPerk) result = true;
        return result;
    }
    public int TallyUpDamage() => dmgBase + dmgPerk + dmgWeapon;
}
public enum DamageType
{
    Blunt,
    Slash,
    Stab
}
