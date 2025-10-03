namespace Framework.Core;

public class SequenceComparer<T>(IEqualityComparer<T> equalityComparer) : IEqualityComparer<IEnumerable<T>>
{

    public bool Equals (IEnumerable<T> x, IEnumerable<T> y)
    {
        return x.SequenceEqual (y, equalityComparer);
    }

    public int GetHashCode(IEnumerable<T> sequence)
    {
        return 0;
    }


    public static SequenceComparer<T> Default { get; } = new SequenceComparer<T> (EqualityComparer<T>.Default);
}

public class ListComparer<T>(IEqualityComparer<T> equalityComparer) : IEqualityComparer<List<T>>
{
    public bool Equals(List<T> x, List<T> y)
    {
        return x.SequenceEqual(y, equalityComparer);
    }

    public int GetHashCode(List<T> list)
    {
        return list.Count;
    }


    public static ListComparer<T> Default { get; } = new ListComparer<T>(EqualityComparer<T>.Default);
}


public class ArrayComparer<T> : IEqualityComparer<T[]>
{
    private ArrayComparer()
    {

    }


    public bool Equals(T[] x, T[] y)
    {
        return x.SequenceEqual(y);
    }

    public int GetHashCode(T[] array)
    {
        return array.Length;
    }


    public static readonly ArrayComparer<T> Value = new ArrayComparer<T>();
}
