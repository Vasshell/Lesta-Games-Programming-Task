using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class FightDirector : MonoBehaviour
{
    [SerializeField] private CharacterStore _playerCS;
    [SerializeField] private PositionMarker _playerPositionMarker;
    [SerializeField] private PositionMarker _enemyPositionMarker;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] private Character _enemyCharacter;
    [SerializeField] private EnemyDictionary _enemyDictionary;
    private Character _playerCharacter;
    private Character _attacker;
    private Character _attacked;
    private int _turn = 0;

    void Start()
    {
        _playerCharacter =  SpawnCharacter(playerPrefab, _playerPositionMarker);
        _playerCharacter.LoadCharacter(_playerCS);
        _enemyCharacter = SpawnCharacter(_enemyDictionary.RandomEnemy(2),_enemyPositionMarker);
        DecideAttacker();
        PlayTurn();
        Switch();
        PlayTurn();
        Switch();
        PlayTurn();
        Switch();
        PlayTurn();
    }

    private void DecideAttacker()
    {
        if (_enemyCharacter.GetAgility() > _playerCharacter.GetAgility())
        {
            _attacker = _enemyCharacter;
            _attacked = _playerCharacter;
        }
        else
        {
            _attacker = _playerCharacter;
            _attacked = _enemyCharacter;
        }
    }

    private void Switch()
    {
        var holdover = _attacker;
        _attacker = _attacked;
        _attacked = holdover;
    }

    private void PlayTurn()
    {
        if (_attacked.GetAgility() > Random.Range(1, _attacked.GetAgility() + _attacker.GetAgility()))
        {
            Damage damage = _attacker.DealDamage();
            ExecuteAbilitiesOfType(_attacker, _attacked, AbilityType.Offensive, _turn, damage);
            ExecuteAbilitiesOfType(_attacked, _attacker, AbilityType.Defensive, _turn, damage);
            _attacked.TakeDamage(damage);
            Debug.Log($"{_attacker.name} dealt {damage.TallyUpDamage()} damage to {_attacked.name}");
        }
        else Debug.Log($"{_attacker.name} Missed!");
        Debug.Log($"{_attacker.name} health: {_attacker.GetHealth()} {_attacked.name} health: {_attacked.GetHealth()}");
    }

    private Character SpawnCharacter(GameObject prefab, PositionMarker positionMarker)
    {
        var newCharacter = Instantiate(prefab);
        newCharacter.transform.position = positionMarker.GetPosition();
        return newCharacter.GetComponent<Character>();    
    }

    private void ExecuteAbilitiesOfType(Character character, Character other, AbilityType abilityType, int turn, Damage damage)
    {
        List<Ability> abilities = character.FindAllAbilitiesOfType(abilityType);
        foreach (Ability ability in abilities)
        {
            damage = ability.FightAbilityCall(turn, character, other, damage); 
        }
    }
}
