using UnityEngine;
using System;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StringEventChannel", menuName = "Scriptable Objects/StringEventChannel")]
public class StringEventChannel : ScriptableObject
{
    public Action<string> Event;

    public void RaiseEvent(string parameter)
    {
        Event.Invoke(parameter);
    }

}
