using System.Collections;

namespace Framework.Core;

public class DeepEqualsCollection<T> : IReadOnlyList<T>, IEquatable<DeepEqualsCollection<T>>
{
    private readonly IReadOnlyList<T> baseSource;

    public DeepEqualsCollection(IEnumerable<T> baseSource, bool ordered = true)
    {
        this.baseSource = (ordered ? baseSource.OrderBy(v => v) : baseSource).ToList();
    }

    public int Count => this.baseSource.Count;

    public T this[int index] => this.baseSource[index];

    public IEnumerator<T> GetEnumerator() => this.baseSource.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public bool Equals(DeepEqualsCollection<T> other) =>
        !object.ReferenceEquals(other, null) && this.baseSource.SequenceEqual(other.baseSource);

    public override bool Equals(object obj)
    {
        if (object.ReferenceEquals(null, obj)) return false;
        if (object.ReferenceEquals(this, obj)) return true;

        return this.Equals(obj as DeepEqualsCollection<T>);
    }

    public override int GetHashCode() => this.baseSource.Count;
}
