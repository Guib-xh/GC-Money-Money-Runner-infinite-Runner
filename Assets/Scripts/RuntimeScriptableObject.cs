using System;
using UnityEngine;

public abstract class RuntimeScriptableObject<T> : ScriptableObject
{
    public event Action<T> OnChanged;
    
    [SerializeField] private T _value;
    
    public T Value
    {
        get => _value;
        set
        {
            _value = value;
            OnChanged?.Invoke(value);
        }
    }
}