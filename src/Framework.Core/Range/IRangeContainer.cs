// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface IRangeContainer<T>
{
    Range<T> Range { get; }
}
