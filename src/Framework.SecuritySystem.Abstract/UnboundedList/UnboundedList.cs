using System.Collections;

namespace Framework.Core;

public class UnboundedList<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;


    private UnboundedList()
    {

    }

    public UnboundedList(IEnumerable<T> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        this._items = items.ToArray();
    }



    public bool IsInfinity
    {
        get { return this._items == null; }
    }

    public bool IsEmpty
    {
        get { return !this.IsInfinity && !this.Any(); }
    }


    public IEnumerator<T> GetEnumerator()
    {
        if (this.IsInfinity)
        {
            throw new InvalidOperationException("Can't iterate infinity list");
        }

        return this._items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }


    public static readonly UnboundedList<T> Infinity = new UnboundedList<T>();

    public static readonly UnboundedList<T> Empty = new UnboundedList<T>(Array.Empty<T>());
}

public static class UnboundedList
{
    public static UnboundedList<T> Yeild<T>(T value)
    {
        return new[] { value}.ToUnboundedList();
    }
}
