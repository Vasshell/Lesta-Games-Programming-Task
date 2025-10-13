using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightDirector : MonoBehaviour
{
    [SerializeField] private CharacterStore _playerCS;
    [SerializeField] private WeaponStore _weaponCS;
    [SerializeField] private PositionMarker _playerPositionMarker;
    [SerializeField] private PositionMarker _enemyPositionMarker;
    [SerializeField] private UIPositionMarker _playerHealthPositionMarker;
    [SerializeField] private UIPositionMarker _enemyHealthPositionMarker;
    [SerializeField] private UIPositionMarker _battleLogPositionMarker;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject effectPrefab;
    [SerializeField] private EnemyDictionary _enemyDictionary;
    [SerializeField] private StringEventChannel _channel;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GameObject _uiTextPrefab;
    [SerializeField] private List<AudioSource> _audioSources;


    private UIText _battleLog;
    private UIText _playerHealth;
    private UIText _enemyHealth;
    private Character playerCharacter;
    private Character enemyCharacter;
    private GameObject _playerObject;
    private GameObject _enemyObject;
    private GameObject[] _attackOrder = new GameObject[2];
    private Dictionary<GameObject, int> _turn;
    private bool _fightWon;

    public IEnumerator Begin(int difficulty)
    {
        _battleLog = Instantiate(_uiTextPrefab, _canvas.transform).GetComponent<UIText>();
        _battleLog.AssignCoordinates(_battleLogPositionMarker.GetPosition());

        _playerHealth = Instantiate(_uiTextPrefab, _canvas.transform).GetComponent<UIText>();
        _playerHealth.AssignCoordinates(_playerHealthPositionMarker.GetPosition());

        _enemyHealth = Instantiate(_uiTextPrefab, _canvas.transform).GetComponent<UIText>();
        _enemyHealth.AssignCoordinates(_enemyHealthPositionMarker.GetPosition());

        _playerObject =  SpawnCharacter(playerPrefab, _playerPositionMarker);
        playerCharacter = _playerObject.GetComponent<Character>();
        playerCharacter.LoadCharacter(_playerCS);
        _enemyObject = SpawnCharacter(_enemyDictionary.RandomEnemy(difficulty),_enemyPositionMarker);
        enemyCharacter = _enemyObject.GetComponent<Character>();
        _turn = new Dictionary<GameObject, int>()
        {
            {_playerObject, 1},
            {_enemyObject, 1},
        };
        DecideAttacker(playerCharacter, enemyCharacter);

        _playerHealth.AssignText($"Здоровье {playerCharacter.GetHealth()}");
        _enemyHealth.AssignText($"Здоровье {enemyCharacter.GetHealth()}");
        _battleLog.AssignText("Начало битвы...");
        yield return new WaitForSeconds(1);
        StartCoroutine(Fight(playerCharacter, enemyCharacter));
    }

    IEnumerator Fight(Character playerCharacter, Character enemyCharacter)
    {
        while (playerCharacter.GetHealth() > 0 && enemyCharacter.GetHealth() > 0)
        {
            yield return StartCoroutine(PlayTurn(_attackOrder[0], _attackOrder[1]));
            Switch();
            yield return null;
        }
        if (playerCharacter.GetHealth() > 0)
        {
            _fightWon = true;
            _weaponCS.SetWeapon(enemyCharacter.GetWeapon());
        }
        else
        {
            _fightWon = false;
        }
        yield return StartCoroutine(FightEnd());
    }

    private IEnumerator FightEnd()
    {
        if (_fightWon)
        {
            _enemyObject.GetComponent<Animator>().SetBool("IsDead", true);
            Instantiate(effectPrefab).transform.position = _enemyObject.transform.position;
            _audioSources.Find(audio => audio.name == "EnemyDeathAudio").Play();
            yield return new WaitForSeconds(3);
            _channel.RaiseEvent("victory");
        }
        else
        {
            _playerObject.GetComponent<Animator>().SetBool("IsDead", true);
            _audioSources.Find(audio => audio.name == "PlayerDeathAudio").Play();
            yield return new WaitForSeconds(3);
            _channel.RaiseEvent("defeat");
        }
    }

    private void Switch()
    {
        var holdover = _attackOrder[0];
        _attackOrder[0] = _attackOrder[1];
        _attackOrder[1] = holdover;
    }

    private void DecideAttacker(Character playerCharacter, Character enemyCharacter)
    {
        if (enemyCharacter.GetAgility() > playerCharacter.GetAgility())
        {
            _attackOrder[1] = _playerObject;
            _attackOrder[0] = _enemyObject;
        }
        else
        {
            _attackOrder[0] = _playerObject;
            _attackOrder[1] = _enemyObject;
        }
    }

    private IEnumerator PlayAttackAnimation(GameObject attackerObject, GameObject attackedObject)
    {
        var attackerAnimator = attackerObject.GetComponent<Animator>();
        var attackedAnimator = attackedObject.GetComponent<Animator>();
        attackerAnimator.SetTrigger("IsAttacking");
        attackedAnimator.SetTrigger("IsDamaged");
        _audioSources.Find(audio => audio.name == "AttackAudio").Play();
        StartCoroutine(MoveToPositionAndBack(attackerObject, attackerObject.transform.position.x));
        yield return new WaitForSeconds(1);
    }

    private IEnumerator MoveToPositionAndBack(GameObject gameObject, float position)
    {
        float increment = (gameObject.transform.position.x - position)/3;
        while (gameObject.transform.position.x != position)
        {
            gameObject.transform.Translate(new Vector3(increment, 0));
            yield return null;
        }
        gameObject.transform.Translate(new Vector3(-increment, 0));
    }

    private IEnumerator PlayTurn(GameObject attackerObject, GameObject attackedObject)
    {
        var attacker = attackerObject.GetComponent<Character>();
        var attacked = attackedObject.GetComponent<Character>();
        if (attacked.GetAgility() < Random.Range(1, attacked.GetAgility() + attacker.GetAgility() + 1))
        {
            _battleLog.AssignText($"{attacker.GetName()} Атакует!");
            yield return new WaitForSeconds(1);
            Damage damage = attacker.DealDamage();
            yield return ExecuteAbilitiesOfType(attacker, attacked, AbilityType.Offensive, _turn[attackerObject], damage);
            yield return ExecuteAbilitiesOfType(attacked, attacker, AbilityType.Defensive, _turn[attackerObject], damage);
            attacked.TakeDamage(damage);
            _playerHealth.AssignText($"Здоровье {playerCharacter.GetHealth()}");
            _enemyHealth.AssignText($"Здоровье {enemyCharacter.GetHealth()}");
            _battleLog.AssignText($"{attacker.GetName()} нанес {damage.TallyUpDamage()} урона {attacked.GetName()}");
            _turn[attackerObject] += 1;
            yield return StartCoroutine(PlayAttackAnimation(attackerObject, attackedObject));
        }
        else
        {
            _audioSources.Find(audio => audio.name == "MissAudio").Play();
            _battleLog.AssignText($"{attacker.GetName()} Промахнулся!");
            yield return new WaitForSeconds(1);
        } 
    }

    private GameObject SpawnCharacter(GameObject prefab, PositionMarker positionMarker)
    {
        var newCharacter = Instantiate(prefab);
        newCharacter.transform.position = positionMarker.GetPosition();
        return newCharacter;    
    }

    private IEnumerator ExecuteAbilitiesOfType(Character character, Character other, AbilityType abilityType, int turn, Damage damage)
    {
        List<Ability> abilities = character.FindAllAbilitiesOfType(abilityType);
        foreach (Ability ability in abilities)
        {
            var damageold = new Damage(damage);
            damage = ability.FightAbilityCall(turn, character, other, damage);
            if (damageold.Compare(damage))
            {
                _battleLog.AssignText($"{character.GetName()}: {ability.title}!");
            }
            yield return new WaitForSeconds(1);
        }
    }
}
