
using System;
using UnityEngine;

public class RuntimeScriptableEvent<T> : ScriptableObject

{
    public event Action<T> Event;

    public void Dispatch(T value)
    {
        Event?.Invoke(value);
    }
}

public class RuntimeScriptableEvent : ScriptableObject

{
    public event Action Event;

    public void Dispatch()
    {
        Event?.Invoke();
    }
}

