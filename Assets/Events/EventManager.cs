using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using Events;
using System;

public class EventManager : MonoBehaviour
{
    private Dictionary<Type, List<WeakReference<IGenericEventListener>>> eventDictionary;
    private static EventManager eventManager;
    public List<IGenericEvent> queue = new List<IGenericEvent>();

    /*public void Update()
    {
        EmptyQueue();
    }*/

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }

            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<Type, List<WeakReference<IGenericEventListener>>>();
        }
    }

    public void RegisterListener<T>(IGenericEventListener listener) where T : IGenericEvent
    {
        StartListening(typeof(T), listener);
    }

    private void StartListening(Type type, IGenericEventListener listener)
    {
        InitiateListener(type);

        if(ContainsListener(eventDictionary[type], listener))
        {
            throw new Exception("Listener already exists for: " + type.FullName);
        }
    }

    public void UnregisterListener(IGenericEventListener listener)
    {
        foreach (Type type in eventDictionary.Keys)
        {
            StopListening(type, listener);
        }
    }

    private bool StopListening(Type type, IGenericEventListener listener)
    {
        if (eventManager == null) return false;

        InitiateListener(type);

        foreach(WeakReference<IGenericEventListener> listen in eventDictionary[type])
        {
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if(check != null)
            {
                if(check == listener)
                {
                    eventDictionary[type].Remove(listen);
                    return true;
                }
            }
        }
        return false;
    }

    public void QueueEvent(IGenericEvent evt)
    {
        queue.Add(evt);
    }

    public void EmptyQueue()
    {
        while(queue.Count > 0)
        {
            TriggerEvent(queue[0]);
            queue.RemoveAt(0);
        }
    }

    public bool TriggerEvent(IGenericEvent evt)
    {
        InitiateListener(evt.GetType());

        for (int i = eventDictionary[evt.GetType()].Count - 1; i >= 0; i--)
        {
            if (i >= eventDictionary[evt.GetType()].Count)
                continue;

            WeakReference<IGenericEventListener> listen = eventDictionary[evt.GetType()][i];
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if(check == null)
            {
                Type type = evt.GetType();
                UnregisterListener(check);
                continue;
            }
            if (check.HandleEvent(evt))
            {
                return true;
            }
        }
        return false;
    }

    private void InitiateListener(Type type)
    {
        if(!eventDictionary.ContainsKey(type))
        {
            eventDictionary.Add(type, new List<WeakReference<IGenericEventListener>>());
        }
    }

    private bool ContainsListener(List<WeakReference<IGenericEventListener>> LISTeners, IGenericEventListener listener)
    {
        foreach(WeakReference<IGenericEventListener> listen in LISTeners)
        {
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if(check != null)
            {
                if (check == listener)
                    return true;
            }
        }
        return false;
    }
}