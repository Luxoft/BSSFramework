using System.Collections.Immutable;

using Framework.Application.Events;

namespace Framework.BLL.Events;

public record TypeEvent(Type Type, ImmutableArray<EventOperation> Operations, Func<object, bool> IsSaveProcessingFunc, Func<object, bool> IsRemoveProcessingFunc)
{
    public static TypeEvent SaveAndRemove<T>(Func<T, bool>? isSaveProcessingFunc = null, Func<T, bool>? isRemoveProcessingFunc = null) =>
        Create(
            [EventOperation.Save, EventOperation.Remove],
            isSaveProcessingFunc,
            isRemoveProcessingFunc);

    public static TypeEvent Save<T>(Func<T, bool>? isSaveFunc = null) => Create(EventOperation.Save, isSaveFunc);

    public static TypeEvent Create<T>(
        EventOperation domainObjectEvent,
        Func<T, bool>? isSaveProcessingFunc = null,
        Func<T, bool>? isRemoveProcessingFunc = null) =>
        Create([domainObjectEvent], isSaveProcessingFunc, isRemoveProcessingFunc);

    public static TypeEvent Create<T>(
        IEnumerable<EventOperation> eventOperations,
        Func<T, bool>? isSaveProcessingFunc = null,
        Func<T, bool>? isRemoveProcessingFunc = null)
    {
        Func<object, bool> defaultFunc = _ => true;

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

        return new TypeEvent(typeof(T), [.. eventOperations], isSaveUntypedFunc, isRemoveUntypeFunc);
    }
}
