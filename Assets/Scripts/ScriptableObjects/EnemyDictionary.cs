using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDictionary", menuName = "Scriptable Objects/EnemyDictionary")]
public class EnemyDictionary : ScriptableObject
{
    [SerializeField] private List<GameObject> enemies;

    public GameObject RandomEnemy(int difficulty)
    {
        difficulty = Mathf.Min(difficulty, enemies.Count);
        int difficultySeed = Mathf.Min(enemies.Count/2+difficulty, enemies.Count);
        int enemyIndex = Random.Range(difficulty, difficultySeed);
        return enemies[enemyIndex];
    }
}
