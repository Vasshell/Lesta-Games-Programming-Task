
using UnityEngine.Events;

public enum Type
{
    StatCompare,
    TurnDependent,
    Permanent
}

[System.Serializable]
public class Ability {
    public string name { get; private set; }
    public string description { get; private set; }
    public Type type;
    public UnityAction<int, Character, Character> AbilityCalled;

    public Ability(string name, string description, Type type)
    {
        this.name = name;
        this.description = description;
        this.type = type;
    }
}
