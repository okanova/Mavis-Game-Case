using System;
using System.Collections.Generic;

public static class EventManager
{
    private static Dictionary<GameEvents, Delegate> eventDictionary = new Dictionary<GameEvents, Delegate>();
    
    public static void RegisterEvent(GameEvents eventType, Action listener)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate existing))
        {
            if (existing is Action action)
                eventDictionary[eventType] = action + listener;
            else
            {
                eventDictionary[eventType] = listener;
            }
        }
        else
        {
            eventDictionary[eventType] = listener;
        }
    }

    public static void UnregisterEvent(GameEvents eventType, Action listener)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate existing) && existing is Action action)
        {
            action -= listener;
            eventDictionary[eventType] = action;
        }
    }

    public static void InvokeEvent(GameEvents eventType)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate del))
        {
            if (del is Action action)
                action.Invoke();
            else
                throw new InvalidOperationException($"Event {eventType} is not parametresiz Action.");
        }
    }
    
    public static void RegisterEvent<T>(GameEvents eventType, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate existing))
        {
            if (existing is Action<T> action)
                eventDictionary[eventType] = action + listener;
            else
            {
                eventDictionary[eventType] = listener;
            }
        }
        else
        {
            eventDictionary[eventType] = listener;
        }
    }

    public static void UnregisterEvent<T>(GameEvents eventType, Action<T> listener)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate existing) && existing is Action<T> action)
        {
            action -= listener;
            eventDictionary[eventType] = action;
        }
    }

    public static void InvokeEvent<T>(GameEvents eventType, T param)
    {
        if (eventDictionary.TryGetValue(eventType, out Delegate del))
        {
            if (del is Action<T> action)
                action.Invoke(param);
            else
                throw new InvalidOperationException($"Event {eventType} is not of type Action<{typeof(T).Name}>.");
        }
    }
    
    public static void ClearAllEvents()
    {
        eventDictionary.Clear();
    }

    public static void ListAllEvents()
    {
        foreach (var evt in eventDictionary.Keys)
        {
            Console.WriteLine($"Registered Event: {evt}");
        }
    }
}
