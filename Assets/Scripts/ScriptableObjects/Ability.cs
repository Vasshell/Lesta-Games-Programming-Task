
using UnityEngine;
using UnityEngine.Events;

public enum AbilityType
{
    Offensive,
    Defensive,
    Permanent
}

[System.Serializable]
[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public class Ability: ScriptableObject {
    [SerializeField] public string idName;
    [SerializeField] public string upgradeTitle;
    [SerializeField] public string title;
    [SerializeField] [TextArea(5,10)] public string description;
    [SerializeField] public AbilityType type;
    [SerializeField] public string nextAbility = null;
    [SerializeField] private functions perkFunction;
    public delegate Damage FightAbilityDelegate(int turn, Character attacker, Character attacked, Damage damage);
    public delegate void StatAbilityDelegate(Character character);
    public StatAbilityDelegate StatAbilityCall;
    public FightAbilityDelegate FightAbilityCall;


    private void OnEnable()
    {
        if (type == AbilityType.Permanent)
        {
            switch (perkFunction)
            {
                case functions.Berserker3:
                    StatAbilityCall = BerserkerThree;
                    break;
                case functions.Warrior3:
                    StatAbilityCall = WarriorThree;
                    break;
                case functions.Rogue2:
                    StatAbilityCall = RogueTwo;
                    break;
            }
        }
        else
        {
            switch (perkFunction)
            {
                case functions.SneakAttack:
                    FightAbilityCall = SneakAttackAbility;
                    break;
                case functions.Poison:
                    FightAbilityCall = PoisonAbility;
                    break;
                case functions.CallToAction:
                    FightAbilityCall = CallToActionAbility;
                    break;
                case functions.Shield:
                    FightAbilityCall = ShieldAbility;
                    break;
                case functions.SkinOfStone:
                    FightAbilityCall= SkinOfStoneAbility;
                    break;
                case functions.Rage:
                    FightAbilityCall = RageAbility;
                    break;
                case functions.Skeleton:
                    FightAbilityCall = SkeletonAbility;
                    break;
                case functions.Slime:
                    FightAbilityCall = SlimeAbility;
                    break;
                case functions.Dragon:
                    FightAbilityCall = DragonAbility;
                    break;
            }
        }
    }

    enum functions
    {
        SneakAttack,
        Poison,
        CallToAction,
        Shield,
        SkinOfStone,
        Rage,
        Skeleton,
        Slime,
        Dragon,
        Berserker3,
        Warrior3,
        Rogue2
    }

    private static Damage DragonAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (turn % 3 == 0)
        {
            damage.dmgPerk += 3;
        }
        return damage;
    }

    private static Damage SlimeAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (damage.damageType == DamageType.Slash)
        {
            damage.dmgWeapon = 0;
        }
        return damage;
    }

    private static Damage SkeletonAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (damage.damageType == DamageType.Blunt)
        {
            damage.dmgPerk += damage.dmgWeapon;
        }
        return damage;
    }

    private static Damage RageAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (turn <= 3)
        {
            damage.dmgPerk += 2;
        }
        else damage.dmgPerk -= 1;
        return damage;
    }

    private static Damage SkinOfStoneAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        damage.dmgPerk -= attacked.GetStamina();
        return damage;
    }

    private static Damage ShieldAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (attacked.GetStrength() > attacker.GetStrength())
        {
            damage.dmgPerk -= 3;
        }
        return damage;
    }

    private static Damage CallToActionAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (turn == 1) damage.dmgPerk += damage.dmgWeapon;
        return damage;
    }

    private static Damage PoisonAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        damage.dmgPerk += turn - 1;
        return damage;
    }

    private static Damage SneakAttackAbility(int turn, Character attacker, Character attacked, Damage damage)
    {
        if (attacker.GetAgility() > attacked.GetAgility())
        {
            damage.dmgPerk += 1;
        }
        return damage;
    }

    private static void BerserkerThree(Character character) => character.SetStamina(character.GetStamina() + 1);
    private static void WarriorThree(Character character) => character.SetStrength(character.GetStrength() + 1);
    private static void RogueTwo(Character character) => character.SetAgility(character.GetAgility() + 1);
}
