using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Validation;

public abstract class ValidationMapBase : IValidationMap
{
    private readonly IDictionaryCache<Type, IClassValidationMap> _cache;


    protected ValidationMapBase(IServiceProvider serviceProvider)
    {
        this.ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        this.AvailableValues = LazyInterfaceImplementHelper.CreateProxy(() => this.ServiceProvider.GetRequiredService<IAvailableValues>());


        this._cache = new LazyImplementDictionaryCache<Type, IClassValidationMap>(type =>
                                                                                  {
                                                                                      var func = new Func<IClassValidationMap<Ignore>>(this.GetInternalClassMap<Ignore>);

                                                                                      var method = func.CreateGenericMethod(type);

                                                                                      return method.Invoke<IClassValidationMap>(this);
                                                                                  }, type => typeof(IClassValidationMap<>).MakeGenericType(type)).WithLock();
    }


    public IServiceProvider ServiceProvider { get; }

    protected IAvailableValues AvailableValues { get; }


    public IClassValidationMap GetClassMap(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return this._cache[type];
    }

    public IClassValidationMap<TSource> GetClassMap<TSource>()
    {
        return (IClassValidationMap<TSource>)this.GetClassMap(typeof(TSource));
    }

    protected IClassValidationMap<TSource> GetClassMap<TSource>(bool lazy)
    {
        return lazy ? LazyInterfaceImplementHelper.CreateProxy(() => this.GetClassMap<TSource>())
                       : this.GetClassMap<TSource>();
    }

    protected abstract IClassValidationMap<TSource> GetInternalClassMap<TSource>();
}
