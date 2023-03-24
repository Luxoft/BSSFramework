using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core;

public static class PeriodItemCollectionContainerExtensions
{
    public static TItem GetNewestItem<TItem> (this IPeriodItemCollectionContainer<TItem> source)
            where TItem : IPeriodObject
    {
        var request = from item in source.Items
                      orderby item.Period descending
                      select item;

        return request.FirstOrDefault ();
    }


    public static TItem SingleOrDefault<TItem> (this IPeriodItemCollectionContainer<TItem> source, DateTime selectedDate)
            where TItem : class, IPeriodObject
    {
        return source.Items.SingleOrDefault (item => item.Period.Contains (selectedDate));
    }
}
