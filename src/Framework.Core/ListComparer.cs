namespace Framework.Core;

public class ListComparer<T>(IEqualityComparer<T> equalityComparer) : IEqualityComparer<IReadOnlyList<T>>
{
    public bool Equals(IReadOnlyList<T>? x, IReadOnlyList<T>? y) => ReferenceEquals(x, y)
                                                                    || (x is not null && y is not null && x.Count == y.Count && x.SequenceEqual(y, equalityComparer));

    public int GetHashCode(IReadOnlyList<T> list) => list.Count;

    public static ListComparer<T> Default { get; } = new(EqualityComparer<T>.Default);
}
