namespace Framework.Core;

public interface IPeriodItemCollectionContainer<out T>
        where T : IPeriodObject
{
    IEnumerable<T> Items { get; }
}
