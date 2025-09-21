using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Events;

public class StringEventListener : MonoBehaviour
{
    [SerializeField] private StringEventChannel _channel;
    public UnityEvent<string> EventRaised;

    private void OnEnable()
    {
        if (_channel != null) 
        {
            _channel.Event += Respond;
        }
    }

    private void OnDisable()
    {
        if (_channel != null)
        {
            _channel.Event -= Respond;
        }
    }

    public void Respond(string parameter)
    {
        EventRaised?.Invoke(parameter);
    }
}
