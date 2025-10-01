using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class UpgradeDirector : MonoBehaviour
{
    [SerializeField] Character _playerCharacter;
    [SerializeField] UpgradeUIDirector _upgradeUIDirector;
    [SerializeField] AbilitiesDictionary _abilitiesDictionary;
    [SerializeField] StringEventChannel _channel;

    private void Start()
    {
        if (GameDirector.gameState == GameDirector.GameState.NewGame)
        {
            _playerCharacter.NewPlayerCharacter();
        }
        DisplayStats();
        DisplayAbilitiesAvailable();
    }

    private void DisplayStats()
    {
        _upgradeUIDirector.DisplayStatNumbers(_playerCharacter.GetStats());
    }

    private void DisplayAbilitiesAvailable()
    {
        if (_playerCharacter.GetLevel() < 3)
        {
            _upgradeUIDirector.DisplayButton(delegate { AddRogueAbility(_playerCharacter.GetLevels().rogue +1); }, _abilitiesDictionary["ability1"].name);
            _upgradeUIDirector.DisplayButton(delegate { AddBerserkerAbility(_playerCharacter.GetLevels().berserker +1); }, _abilitiesDictionary["ability1"].name);
            _upgradeUIDirector.DisplayButton(delegate { AddWarriorAbility(_playerCharacter.GetLevels().warrior +1); }, _abilitiesDictionary["ability1"].name);
        }
    }
    
    private void DisplayWeaponAvailable()
    {
        
    }

    private void AddWarriorAbility(int level)
    {
        Debug.Log("Warrior " + level);
        _playerCharacter.SetWarriorLevel(level);
        AbilityChosen();
    }

    private void AddRogueAbility(int level)
    {
        Debug.Log("Rogue " + level);
        _playerCharacter.SetRogueLevel(level);
        AbilityChosen();
    }

    private void AddBerserkerAbility(int level)
    {
        Debug.Log("Berserker " + level);
        _playerCharacter.SetBerserkerLevel(level);
        AbilityChosen();

    }

    private void AddWeapon()
    {

    }

    private void AbilityChosen()
    {
        _playerCharacter.LevelUp();
        _upgradeUIDirector.RemoveButtons();
        _upgradeUIDirector.DisplayButton(delegate { _channel.RaiseEvent("fight"); }, "Продолжить");
    }
}
