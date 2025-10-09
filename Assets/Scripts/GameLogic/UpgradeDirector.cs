using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class UpgradeDirector : MonoBehaviour
{
    [SerializeField] UpgradeUIDirector _upgradeUIDirector;
    [SerializeField] AbilitiesDictionary _abilitiesDictionary;
    [SerializeField] StringEventChannel _channel;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] private WeaponDictionary _weaponDictionary;
    [SerializeField] private PositionMarker _playerPositionMarker;
    [SerializeField] private CharacterStore _characterStore;
    [SerializeField] private List<string> _defaultAbilities;

    private GameObject _playerObject;
    private Character _playerCharacter;

    private void Start()
    {
        SpawnPlayer();
        if (GameDirector.gameState == GameDirector.GameState.NewGame)
        {
            _playerCharacter.NewPlayerCharacter();
        }
        else
        {
            _playerCharacter.LoadCharacter(_characterStore);
        }
        DisplayStats();
        DisplayAbilitiesAvailable(GetAbilitiesAvailable(_defaultAbilities));
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
                _upgradeUIDirector.DisplayButton(delegate { AddAbility(ability); }, ability.title, ability.description);
            }
        }
    }
    
    private void DisplayWeaponAvailable()
    {
        
    }

    private void AddAbility(Ability ability)
    {
        if (ability.type == AbilityType.Permanent)
        {
            _playerCharacter.ApplyPermanentAbility(ability);
        }
        else _playerCharacter.AddAbility(ability);
        if (GameDirector.gameState == GameDirector.GameState.NewGame)
        {
            switch (ability.idName)
            {
                case "rogue1":
                    _playerCharacter.SetWeapon(_weaponDictionary["dagger"]);
                        break;
                
                case "warrior1": _playerCharacter.SetWeapon(_weaponDictionary["sword"]);
                        break;
                
                case "berserker1": _playerCharacter.SetWeapon(_weaponDictionary["club"]);
                        break;
                
            }
        }
        if (ability.idName.StartsWith("rogue")) _playerCharacter.SetHealth(_playerCharacter.GetHealth()+4);
        if (ability.idName.StartsWith("warrior")) _playerCharacter.SetHealth(_playerCharacter.GetHealth()+5);
        if (ability.idName.StartsWith("berserker")) _playerCharacter.SetHealth(_playerCharacter.GetHealth()+6);
        DisplayStats();
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

    private void ExitMenu()
    {
        _characterStore.SetCharacter(_playerCharacter);
        _channel.RaiseEvent("fight");
    }
}
