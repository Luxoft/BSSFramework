#nullable enable

namespace Framework.Core;

public readonly record struct MergeResult<TSource, TTarget>(
    IReadOnlyList<TTarget> AddingItems,
    IReadOnlyList<ValueTuple<TSource, TTarget>> CombineItems,
    IReadOnlyList<TSource> RemovingItems)
{
    public MergeResult(
        IEnumerable<TTarget> addingItems,
        IEnumerable<ValueTuple<TSource, TTarget>> combineItems,
        IEnumerable<TSource> removingItems)
        : this(addingItems.ToList(), combineItems.ToList(), removingItems.ToList())
    {
    }

    public bool IsEmpty => !this.RemovingItems.Any() && !this.AddingItems.Any();

    public static readonly MergeResult<TSource, TTarget> Empty = new([], [], []);
}

public static class MergeResultExtensions
{
    public static MergeResult<TResult, TResult> Select<TSource, TResult>(
        this MergeResult<TSource, TSource> mergeResult,
        Func<TSource, TResult> selector) =>
        new(
            mergeResult.AddingItems.Select(selector),
            mergeResult.CombineItems.Select(t => (selector(t.Item1), selector(t.Item2))),
            mergeResult.RemovingItems.Select(selector));

    public static MergeResult<TSource, TSource> Where<TSource>(
        this MergeResult<TSource, TSource> mergeResult,
        Func<TSource, bool> filter) =>
        new(
            mergeResult.AddingItems.Where(filter),
            mergeResult.CombineItems.Where(t => filter(t.Item1) && filter(t.Item2)),
            mergeResult.RemovingItems.Where(filter));

    public static MergeResult<TSource, TTarget> Concat<TSource, TTarget>(
        this MergeResult<TSource, TTarget> m1,
        MergeResult<TSource, TTarget> m2) =>
        new(
            m1.AddingItems.Concat(m2.AddingItems),
            m1.CombineItems.Concat(m2.CombineItems),
            m1.RemovingItems.Concat(m2.RemovingItems));

    public static MergeResult<TNewSource, TTarget> ChangeSource<TSource, TTarget, TNewSource>(
        this MergeResult<TSource, TTarget> mergeResult,
        Func<TSource, TNewSource> selector) =>
        new(
            mergeResult.AddingItems,
            mergeResult.CombineItems.Select(t => (selector(t.Item1), t.Item2)),
            mergeResult.RemovingItems.Select(selector));

    public static MergeResult<TSource, TNewTarget> ChangeTarget<TSource, TTarget, TNewTarget>(
        this MergeResult<TSource, TTarget> mergeResult,
        Func<TTarget, TNewTarget> selector) =>
        new(
            mergeResult.AddingItems.Select(selector),
            mergeResult.CombineItems.Select(t => (t.Item1, selector(t.Item2))),
            mergeResult.RemovingItems);
}
