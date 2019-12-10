using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CollisionListener : MonoBehaviour, IGenericEventListener
{
    public bool HandleEvent(IGenericEvent evt)
    {
        Debug.Log("Event Listened to!");
        return true;
    }

    private void Start()
    {
        EventManager.instance.RegisterListener<CollisionEvent>(this);
    }
}
