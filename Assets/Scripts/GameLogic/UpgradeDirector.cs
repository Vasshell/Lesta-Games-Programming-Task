using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.Android;

public class UpgradeDirector : MonoBehaviour
{
    [SerializeField] UpgradeUIDirector _upgradeUIDirector;
    [SerializeField] AbilitiesDictionary _abilitiesDictionary;
    [SerializeField] StringEventChannel _channel;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] private WeaponDictionary _weaponDictionary;
    [SerializeField] private PositionMarker _playerPositionMarker;
    [SerializeField] private CharacterStore _characterStore;
    [SerializeField] private WeaponStore _weaponStore;
    [SerializeField] private List<string> _defaultAbilities;
    private GameState _gameState;
    private GameObject _playerObject;
    private Character _playerCharacter;

    public IEnumerator Begin(GameState gameState)
    {
        yield return StartCoroutine(_upgradeUIDirector.Begin());
        _gameState = gameState;
        SpawnPlayer();
        if (_gameState == GameState.NewGame)
        {
            _playerCharacter.NewPlayerCharacter();
        }
        else
        {
            _playerCharacter.LoadCharacter(_characterStore);
            DisplayWeaponAvailable(_weaponStore.GetWeapon());
        }
        DisplayStats();
        DisplayAbilitiesAvailable(GetAbilitiesAvailable(_defaultAbilities));
        yield break;
    }

    private void Update()
    {
        DisplayStats();
    }

    private void SpawnPlayer()
    {
        _playerObject = Instantiate(playerPrefab);
        _playerObject.transform.position = _playerPositionMarker.GetPosition();
        _playerCharacter = _playerObject.GetComponent<Character>();
    }

    private void DisplayStats()
    {
        _upgradeUIDirector.DisplayStatNumbers(_playerCharacter.GetStats());
    }

    private void DisplayAbilitiesAvailable(List<Ability> abilitiesAvailable)
    {
        if (_playerCharacter.GetLevel() < 3)
        {
            foreach (Ability ability in abilitiesAvailable)
            {
                _upgradeUIDirector.DisplayButton(delegate { AddAbility(ability); }, ability.upgradeTitle, ability.title+": "+ability.description);
            }
        }
        else _upgradeUIDirector.DisplayButton(delegate { ExitMenu(); }, "Продолжить");
    }
    
    private void DisplayWeaponAvailable(Weapon weapon)
    {
        _upgradeUIDirector.DisplayWeaponButton(weapon, delegate { WeaponChosen(weapon); }, _playerCharacter.GetWeapon());
    }

    private void AddAbility(Ability ability)
    {
        if (ability.type == AbilityType.Permanent)
        {
            _playerCharacter.ApplyPermanentAbility(ability);
        }
        else _playerCharacter.AddAbility(ability);
        if (_gameState == GameState.NewGame)
        {
            switch (ability.idName)
            {
                case "rogue1":
                    DisplayWeaponAvailable(_weaponDictionary["dagger"]);
                        break;
                
                case "warrior1":
                    DisplayWeaponAvailable(_weaponDictionary["sword"]);
                    break;
                
                case "berserker1":
                    DisplayWeaponAvailable(_weaponDictionary["club"]);
                    break;
                
            }
        }
        if (ability.idName.StartsWith("rogue")) _playerCharacter.SetHealth(_playerCharacter.GetHealth() + 4);
        if (ability.idName.StartsWith("warrior")) _playerCharacter.SetHealth(_playerCharacter.GetHealth()+5);
        if (ability.idName.StartsWith("berserker")) _playerCharacter.SetHealth(_playerCharacter.GetHealth()+6);
        AbilityChosen();
    }

    private void AddWeapon(Weapon weapon) => _playerCharacter.SetWeapon(weapon);

    private List<Ability> GetAbilitiesAvailable(List<string> abilityNames)
    {
        List<Ability> abilitiesAvailable = new List<Ability>();
        foreach (string abilityName in abilityNames)
        {
            var newAbility = _abilitiesDictionary[_playerCharacter.FindNextAbility(abilityName)];
            abilitiesAvailable.Add(newAbility);
        }
        return abilitiesAvailable;
    }

    private void AbilityChosen()
    {
        _playerCharacter.LevelUp();
        _upgradeUIDirector.RemoveButtons();
        _upgradeUIDirector.DisplayButton(delegate { ExitMenu(); }, "Продолжить");
    }

    private void WeaponChosen(Weapon weapon)
    {
        _playerCharacter.SetWeapon(weapon);
        _upgradeUIDirector.RemoveWeaponButton();
    }

    private void ExitMenu()
    {
        _characterStore.SetCharacter(_playerCharacter);
        _channel.RaiseEvent("fight");
    }
}
