// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface PeriodItemCollectionContainer<out T>
        where T : IPeriodObject
{
    IEnumerable<T> Items { get; }
}
