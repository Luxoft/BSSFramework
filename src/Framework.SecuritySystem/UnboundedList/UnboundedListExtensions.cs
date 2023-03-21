using System.Collections.Generic;
using System.Linq;

namespace Framework.Core;

public static class UnboundedListExtensions
{
    public static UnboundedList<T> Union<T>(this UnboundedList<T> list, UnboundedList<T> otherList, IEqualityComparer<T> equalityComparer = null)
    {
        return list.IsInfinity ? otherList : otherList.IsInfinity ? list : Enumerable.Union(list, otherList, equalityComparer).ToUnboundedList();
    }

    public static UnboundedList<T> Union<T>(this IEnumerable<UnboundedList<T>> lists, IEqualityComparer<T> equalityComparer = null)
    {
        return lists.Aggregate(UnboundedList<T>.Infinity, (list1, list2) => list1.Union(list2, equalityComparer));
    }


    public static UnboundedList<T> Concat<T>(this UnboundedList<T> list, UnboundedList<T> otherList)
    {
        return list.IsInfinity || otherList.IsInfinity ? UnboundedList<T>.Infinity : Enumerable.Concat(list, otherList).ToUnboundedList();
    }

    public static UnboundedList<T> Concat<T>(this IEnumerable<UnboundedList<T>> lists)
    {
        return lists.Aggregate(UnboundedList<T>.Empty, (list1, list2) => list1.Concat(list2));
    }


    public static UnboundedList<T> Distinct<T>(this UnboundedList<T> list, IEqualityComparer<T> equalityComparer = null)
    {
        return list.IsInfinity ? UnboundedList<T>.Infinity : Enumerable.Distinct(list, equalityComparer).ToUnboundedList();
    }


    public static UnboundedList<T> ToUnboundedList<T>(this IEnumerable<T> source)
    {
        return new UnboundedList<T>(source);
    }
}
