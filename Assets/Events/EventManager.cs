using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class EventManager : MonoBehaviour
{
    private Dictionary<Type, List<WeakReference<IGenericEventListener>>> eventDictionary;
    public static EventManager eventManager;
    public List<IGenericEvent> queue = new List<IGenericEvent>();
    //public static EventManager instance { get; } = new EventManager();
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
        //Debug.Log("Listener Registered");
        StartListening(typeof(T), listener);
    }

    private void StartListening(Type type, IGenericEventListener listener)
    {
        InitiateListener(type);

        if (eventDictionary != null)
        {
            if (ContainsListener(eventDictionary[type], listener))
            {
                throw new Exception("Listener already exists for: " + type.FullName);
            }
        }
        eventDictionary[type].Insert(0, new WeakReference<IGenericEventListener>(listener));
        Debug.Log("Number of Keys" + eventDictionary.Keys.Count);
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

        foreach (WeakReference<IGenericEventListener> listen in eventDictionary[type])
        {
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if (check != null)
            {
                if (check == listener)
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
        //Debug.Log("Event queue being emptied");
        while (queue.Count > 0)
        {
            TriggerEvent(queue[0]);
            queue.RemoveAt(0);
        }
    }

    public bool TriggerEvent(IGenericEvent evt)
    {
        InitiateListener(evt.GetType());
        //Debug.Log("Event Dictionary Type Count" + eventDictionary[evt.GetType()].Count);

        for (int i = eventDictionary[evt.GetType()].Count - 1; i >= 0; i--)
        {
            Debug.Log("Event Dictionary Type Count" + eventDictionary[evt.GetType()].Count);
            if (i >= eventDictionary[evt.GetType()].Count)
            {
                Debug.Log("Calling Continue");
                continue;
            }

            WeakReference<IGenericEventListener> listen = eventDictionary[evt.GetType()][i];
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if (check == null)
            {
                Debug.Log("Check was null");
                Type type = evt.GetType();
                UnregisterListener(check);
                continue;
            }
            if (check.HandleEvent(evt))
            {
                Debug.Log("Calling HandleEvent");
                return true;
            }
        }
        return false;
    }

    private void InitiateListener(Type type)
    {
        if(!eventDictionary.ContainsKey(type))
        {
            Debug.Log("Type: " + type.ToString() + " registered as listener");
            eventDictionary.Add(type, new List<WeakReference<IGenericEventListener>>());
        }
    }

    private bool ContainsListener(List<WeakReference<IGenericEventListener>> LISTeners, IGenericEventListener listener)
    {
        foreach (WeakReference<IGenericEventListener> listen in LISTeners)
        {
            IGenericEventListener check;
            listen.TryGetTarget(out check);
            if (check != null)
            {
                if (check == listener)
                    return true;
            }
        }
        return false;
    }
    void Update()
    {
        eventManager.EmptyQueue();
    }
}
