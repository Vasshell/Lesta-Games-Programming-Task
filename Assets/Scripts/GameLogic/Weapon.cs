using UnityEngine;
[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon: ScriptableObject
{
    [SerializeField] public string idName;
    [SerializeField] private string wpnName;
    [SerializeField] private int damage;
    [SerializeField] private DamageType damageType;

    public int GetDamage() => damage;
    public DamageType GetDamageType() => damageType;
    public string GetWpnName() => wpnName;
}
