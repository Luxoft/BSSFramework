using System.Collections.ObjectModel;

namespace Framework.Core;

public struct MergeResult<TSource, TTarget>
{
    public IList<TSource> RemovingItems;

    public IList<TTarget> AddingItems;

    public IList<TupleStruct<TSource, TTarget>> CombineItems;


    public bool IsEmpty
    {
        get
        {
            return !this.RemovingItems.Any() && !this.AddingItems.Any();
        }
    }

    public MergeResult(IEnumerable<TTarget> addingItems, IEnumerable<TupleStruct<TSource, TTarget>> combineItems, IEnumerable<TSource> removingItems) : this()
    {
        this.RemovingItems = removingItems.ToList();
        this.AddingItems = addingItems.ToList();
        this.CombineItems = combineItems.ToList();
    }


    public static readonly MergeResult<TSource, TTarget> Empty = new MergeResult<TSource, TTarget>
                                                                 {
                                                                         AddingItems = new ReadOnlyCollection<TTarget>(new TTarget[0]),
                                                                         RemovingItems = new ReadOnlyCollection<TSource>(new TSource[0]),
                                                                         CombineItems = new ReadOnlyCollection<TupleStruct<TSource, TTarget>>(new TupleStruct<TSource, TTarget>[0])
                                                                 };
}

public static class MergeResultExtensions
{
    public static MergeResult<TResult, TResult> Select<TSource, TResult>(this MergeResult<TSource, TSource> mergeResult, Func<TSource, TResult> selector)
    {
        if (selector == null) throw new ArgumentNullException(nameof(selector));

        return new MergeResult<TResult, TResult>
               {
                       AddingItems = mergeResult.AddingItems.ToList(selector),
                       RemovingItems = mergeResult.RemovingItems.ToList(selector),
                       CombineItems = mergeResult.CombineItems.ToList(t => TupleStruct.Create(selector(t.Item1), selector(t.Item2)))
               };
    }

    public static MergeResult<TSource, TSource> Where<TSource>(this MergeResult<TSource, TSource> mergeResult, Func<TSource, bool> filter)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        return new MergeResult<TSource, TSource>
               {
                       AddingItems = mergeResult.AddingItems.Where(filter).ToList(),
                       RemovingItems = mergeResult.RemovingItems.Where(filter).ToList(),
                       CombineItems = mergeResult.CombineItems.Where(t => filter(t.Item1) && filter(t.Item2)).ToList()
               };
    }


    public static MergeResult<TSource, TTarget> Concat<TSource, TTarget>(this MergeResult<TSource, TTarget> m1, MergeResult<TSource, TTarget> m2)
    {
        return new MergeResult<TSource, TTarget>
               {
                       AddingItems = m1.AddingItems.Concat(m2.AddingItems).ToList(),
                       RemovingItems = m1.RemovingItems.Concat(m2.RemovingItems).ToList(),
                       CombineItems = m1.CombineItems.Concat(m2.CombineItems).ToList(),
               };
    }
}
