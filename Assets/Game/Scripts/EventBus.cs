using System;
using System.Collections.Generic;

/// <summary>
/// Generic, decoupled event bus for publishing and subscribing to events.
/// </summary>
public static class EventBus
{
    private static readonly Dictionary<Type, List<Delegate>> _subscribers = new();

    /// <summary>
    /// Subscribe to an event of type T.
    /// </summary>
    public static void Subscribe<T>(Action<T> handler) where T : IEvent
    {
        var type = typeof(T);
        if (!_subscribers.ContainsKey(type))
            _subscribers[type] = new List<Delegate>();
        _subscribers[type].Add(handler);
    }

    /// <summary>
    /// Unsubscribe from an event of type T.
    /// </summary>
    public static void Unsubscribe<T>(Action<T> handler) where T : IEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlers))
        {
            handlers.Remove(handler);
            if (handlers.Count == 0)
                _subscribers.Remove(type);
        }
    }

    /// <summary>
    /// Publish an event of type T to all subscribers.
    /// </summary>
    public static void Publish<T>(T evt) where T : IEvent
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var handlers))
        {
            foreach (var handler in handlers)
            {
                if (handler is Action<T> action)
                    action(evt);
            }
        }
    }
}
