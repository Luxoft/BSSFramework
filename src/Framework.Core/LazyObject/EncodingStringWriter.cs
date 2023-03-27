namespace Framework.Core;

public class LazyObject<T> : Lazy<T>, ILazyObject<T>
{
    public LazyObject(Func<T> valueFactory)
        : base(valueFactory)
    {
    }
}
