// ReSharper disable once CheckNamespace
namespace Framework.Core;

public interface PeriodItemCollectionContainer<out T>
        where T : PeriodObject
{
    IEnumerable<T> Items { get; }
}
