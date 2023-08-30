namespace Framework.Core;

public class SequenceComparer<T> : IEqualityComparer<IEnumerable<T>>
{
    private SequenceComparer ()
    {

    }


    public bool Equals (IEnumerable<T> x, IEnumerable<T> y)
    {
        return x.SequenceEqual (y);
    }

    public int GetHashCode(IEnumerable<T> sequence)
    {
        return 0;
    }


    public static readonly SequenceComparer<T> Value = new SequenceComparer<T> ();
}

public class ListComparer<T> : IEqualityComparer<List<T>>
{
    private ListComparer()
    {

    }


    public bool Equals(List<T> x, List<T> y)
    {
        return x.SequenceEqual(y);
    }

    public int GetHashCode(List<T> list)
    {
        return list.Count;
    }


    public static readonly ListComparer<T> Value = new ListComparer<T>();
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
