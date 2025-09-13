using UnityEngine;

public class Character : MonoBehaviour
{
    public int _health;
    public int _attStrength;
    public int _attAgility;
    public int _attStamina;
    public int _damage;


    // TODO: ��������� �������� � ������, ����. ����� ������ ���������� �������?
    // TODO: ��������� �������� ��������� ������ � ��������� ����� (������ ������)

    void Attack(Character attacked)
    {
        attacked.TakeDamage(_damage);
    }

    void TakeDamage(int recieved_damage)
    {
        _health -= recieved_damage;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
