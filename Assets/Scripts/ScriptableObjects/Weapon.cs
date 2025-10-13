using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Weapon", menuName = "Scriptable Objects/Weapon")]
public class Weapon: ScriptableObject
{
    [SerializeField] public string idName;
    [SerializeField] private string wpnName;
    [SerializeField] private int damage;
    [SerializeField] private DamageType damageType;
    [SerializeField] private Sprite image;

    public int GetDamage() => damage;
    public DamageType GetDamageType() => damageType;
    public string GetWpnName() => wpnName;
    public Sprite GetImage() => image;
}
