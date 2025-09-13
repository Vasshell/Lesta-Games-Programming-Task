using System;
using UnityEngine;
using UnityEngine.Events;

public class GameDirector:MonoBehaviour
{
    public UnityEvent playerTurn;
    public UnityEvent enemyTurn;

    private void Start()
    {
        print($"{this.name}: im a cooklo cukl... cuckold coroche");
        FirstTurnDetermine();
    }

    void FirstTurnDetermine()
    {
        var playerAgility = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>().attAgility;
        var enemyAgility = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Character>().attAgility;
        if (enemyAgility > playerAgility)
        {
            print($"{this.name}: Enemy turn!");
            enemyTurn.Invoke();
        }
        else
        {
            print($"{this.name}: Player turn!");
            playerTurn.Invoke();
            
        }
    }
    enum States
    {
        MainMenu,
        PauseMenu,
        CharCreate,
        CharUpgrade,
        Fight,
        Victory,
        Defeat
    }
}
