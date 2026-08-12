using Framework.Core;

namespace Framework.Validation;

public interface IAvailableValues
{
    Range<T> GetAvailableRange<T>();

    int GetAvailableSize<T>();
}
