using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Scriptable Objects/Abilities")]
public class AbilitiesDictionary : ScriptableObject
{
    public Dictionary<string, Ability> abilities = new Dictionary<string, Ability>
    {
        {"ability1",new Ability("Ability One", "Does Whatever", Type.Permanent){ AbilityCalled = AbilityOne} }
    };

    public static void AbilityOne(int turn, Character attacker, Character attacked)
    {
        Debug.Log("AbilityONEEEEE");
    }

    public Ability this[string name] => abilities[name];
}
