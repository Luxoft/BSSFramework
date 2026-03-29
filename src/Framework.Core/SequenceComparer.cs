namespace Framework.Core;

public class SequenceComparer<T>(IEqualityComparer<T> equalityComparer) : IEqualityComparer<IEnumerable<T>>
{
    public bool Equals (IEnumerable<T>? x, IEnumerable<T>? y) => x.SequenceEqual (y, equalityComparer);

    public int GetHashCode(IEnumerable<T> sequence) => 0;

    public static SequenceComparer<T> Default { get; } = new(EqualityComparer<T>.Default);
}
