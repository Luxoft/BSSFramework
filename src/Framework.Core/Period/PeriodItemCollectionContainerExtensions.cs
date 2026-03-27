// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class PeriodItemCollectionContainerExtensions
{
    public static TItem? GetNewestItem<TItem> (this PeriodItemCollectionContainer<TItem> source)
            where TItem : PeriodObject
    {
        var request = from item in source.Items
                      orderby item.Period descending
                      select item;

        return request.FirstOrDefault ();
    }

    public static TItem? SingleOrDefault<TItem> (this PeriodItemCollectionContainer<TItem> source, DateTime selectedDate)
            where TItem : class, PeriodObject =>
        source.Items.SingleOrDefault (item => item.Period.Contains (selectedDate));
}
