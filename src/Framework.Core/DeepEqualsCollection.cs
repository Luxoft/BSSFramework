#nullable enable

using System.Collections;

namespace Framework.Core;

public class DeepEqualsCollection<T> : IReadOnlyList<T>, IEquatable<DeepEqualsCollection<T>>
{
    private readonly IEqualityComparer<T> comparer;

    private readonly IReadOnlyList<T> baseSource;

    public DeepEqualsCollection(IEnumerable<T> baseSource, IEqualityComparer<T>? comparer = null)
    {
        this.comparer = comparer ?? EqualityComparer<T>.Default;
        this.baseSource = baseSource.ToList();
    }

    public int Count => this.baseSource.Count;

    public T this[int index] => this.baseSource[index];

    public IEnumerator<T> GetEnumerator() => this.baseSource.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public bool Equals(DeepEqualsCollection<T>? other) =>
        !object.ReferenceEquals(other, null) && this.baseSource.SequenceEqual(other.baseSource, this.comparer);

    public override bool Equals(object? obj)
    {
        if (object.ReferenceEquals(null, obj)) return false;
        if (object.ReferenceEquals(this, obj)) return true;

        return this.Equals(obj as DeepEqualsCollection<T>);
    }

    public override int GetHashCode() => this.baseSource.Count;
}

public static class DeepEqualsCollection
{
    public static DeepEqualsCollection<T> Create<T>(IEnumerable<T> source, IEqualityComparer<T> comparer = null)
    {
        return new DeepEqualsCollection<T>(source, comparer);
    }
}
