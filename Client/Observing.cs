using System.Xml.Serialization;
using Client.Game;

public delegate void ObservableEventCallback<TSource, T>(TSource owner, T? from, T? to);
public class ObservableProperty<TSource, T>
{
    public event ObservableEventCallback<TSource, T>? ChangedValue;
    public TSource Owner { get; }
    public ObservableProperty(TSource heldBy, T? initialValue = default)
    {
        Owner = heldBy;
        _value = initialValue;
    }
    private T? _value;
    public T? Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                T? oldValue = _value;
                _value = value;
                ChangedValue?.Invoke(Owner, from: oldValue, to: value);
            }
        }
    }

    /// <summary>
    /// Adds an event handler to the ObservableProperty
    /// </summary>
    //public static ObservableEventCallback<TSource, T>? operator +(ObservableProperty<TSource, T> prop, ObservableEventCallback<TSource, T> handler)
    //{
    //    prop.ChangedValue += handler;
    //    return prop.ChangedValue;
    //}

    /// <summary>
    /// Removes an event handler from the ObservableProperty
    /// </summary>
    //public static ObservableEventCallback<TSource, T>? operator -(ObservableProperty<TSource, T> prop, ObservableEventCallback<TSource, T> handler)
    //{
    //    prop.ChangedValue -= handler;
    //    return prop.ChangedValue;
    //}

    /// <summary>
    /// Implicit conversion from ObservableProperty<TSource, T> to T
    /// </summary>
    public static implicit operator T?(ObservableProperty<TSource, T> prop) => prop.Value;
}
