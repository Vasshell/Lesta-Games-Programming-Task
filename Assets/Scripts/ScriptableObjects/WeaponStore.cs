using UnityEngine;

[CreateAssetMenu(fileName = "WeaponStore", menuName = "Scriptable Objects/WeaponStore")]
public class WeaponStore : ScriptableObject
{
    private Weapon _weapon;

    public void SetWeapon(Weapon weapon) => _weapon = weapon;
    public Weapon GetWeapon() => _weapon;
}
