namespace Framework.Core;

public class DynamicSource : IDynamicSource
{
    private readonly IDictionaryCache<Type, object> _cache;


    public DynamicSource()
            : this(new object[0])
    {

    }

    public DynamicSource(IEnumerable<object> items)
    {
        if (items == null) throw new ArgumentNullException(nameof(items));

        var cachedItems = LazyHelper.Create(() => items.Concat(this.GetItems()).ToArray());

        this._cache = new DictionaryCache<Type, object>(type =>
                                                                cachedItems.Value.Where(type.IsInstanceOfType).SingleOrDefault(duplicate =>
                                                                        new Exception($"More one objects of type {type.Name} founded: {duplicate.Join(", ", t => t.GetType().Name)}")))
                .WithLock();
    }


    protected virtual IEnumerable<object> GetItems()
    {
        yield break;
    }



    public virtual T GetValue<T>()
            where T : class
    {
        return (T)this._cache[typeof(T)];
    }


    public static readonly DynamicSource Empty = new DynamicSource();
}
