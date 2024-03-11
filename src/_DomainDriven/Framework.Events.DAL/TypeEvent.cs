namespace Framework.Events;

/// <summary>
///
/// </summary>
public struct TypeEvent
{
    public static TypeEvent SaveAndRemove<T>(Func<T, bool> isSaveProcessingFunc = null, Func<T, bool> isRemoveProcessingFunc = null)
    {
        return TypeEvent.Create(
            [DomainObjectEvent.Save, DomainObjectEvent.Remove],
            isSaveProcessingFunc,
            isRemoveProcessingFunc);
    }

    public static TypeEvent Save<T>(Func<T, bool> isSaveFunc = null)
    {
        return TypeEvent.Create(DomainObjectEvent.Save, isSaveFunc);
    }

    public static TypeEvent Create<T>(
        DomainObjectEvent domainObjectEvent,
        Func<T, bool> isSaveProcessingFunc = null,
        Func<T, bool> isRemoveProcessingFunc = null)
    {
        return Create(new[] { domainObjectEvent }, isSaveProcessingFunc, isRemoveProcessingFunc);
    }

    public static TypeEvent Create<T>(
        DomainObjectEvent[] eventOperations,
        Func<T, bool> isSaveProcessingFunc = null,
        Func<T, bool> isRemoveProcessingFunc = null)
    {
        Func<object, bool> defaultFunc = z => true;
        var isSaveUntypedFunc = defaultFunc;
        var isRemoveUntypeFunc = defaultFunc;

        if (isSaveProcessingFunc != null)
        {
            isSaveUntypedFunc = z => isSaveProcessingFunc((T)z);
        }

        if (null != isRemoveProcessingFunc)
        {
            isRemoveUntypeFunc = z => isRemoveProcessingFunc((T)z);
        }

        return new TypeEvent(typeof(T), eventOperations, isSaveUntypedFunc, isRemoveUntypeFunc);
    }

    public TypeEvent(Type type, IEnumerable<DomainObjectEvent> operations, Func<object, bool> isSaveProcessingFunc, Func<object, bool> isRemoveProcessingFunc) : this()
    {
        this.Type = type;

        this.Operations = operations.ToList();

        this.IsSaveProcessingFunc = isSaveProcessingFunc;

        this.IsRemoveProcessingFunc = isRemoveProcessingFunc;
    }

    /// <summary>
    ///
    /// </summary>
    public Type Type { get; private set; }
    /// <summary>
    ///
    /// </summary>
    public IReadOnlyList<DomainObjectEvent> Operations { get; }

    public Func<object, bool> IsSaveProcessingFunc { get; }

    public Func<object, bool> IsRemoveProcessingFunc { get; }
}
