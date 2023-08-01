using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomUnityEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField]
    private CustomUnityEvent response;

    private void OnEnable()
    {
        gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(Component sender, object data)
    {
        response.Invoke (sender, data);
    }
}
