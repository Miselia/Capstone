using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class SpawnListener : MonoBehaviour, IGenericEventListener
{
    public bool HandleEvent(IGenericEvent evt)
    {
        Debug.Log("Event Listened to!");
        return true;
    }

    void onSpawn(int cardID, int player)
    {

    }

    private void Start()
    {
        EventManager.instance.RegisterListener<SpawnEvent>(this);
    }
}
