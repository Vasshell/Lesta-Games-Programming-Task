using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class Character : MonoBehaviour
{

    public int health { get; private set; }
    public int attStrength { get; private set; }
    public int attAgility { get; private set; }
    public int damage { get; private set; }   
    public int attStamina { get; private set; }

    // TODO: Прописать здоровье и оружие, урон. Более гибкое назначение классов?
    // TODO: ДОпустить различия персонажа игрока и персонажа врага (разные классы)

    void Attack(Character attacked)
    {
        print($"{this.name}: Attacking!");
        attacked.TakeDamage(damage);
    }

    void TakeDamage(int recieved_damage)
    {
        print($"{this.name}: {recieved_damage} damage taken");
        health -= recieved_damage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 10;
        damage = Random.Range(1, 4);
        attAgility = Random.Range(1, 3);
        attStamina = Random.Range(1, 3);
        attStrength = Random.Range(1, 3);
        print($"{this.name}: All set!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMyTurn(Character combatant)
    {
        print($"It's {this.name} turn!");
        Attack(combatant);
    }

}
