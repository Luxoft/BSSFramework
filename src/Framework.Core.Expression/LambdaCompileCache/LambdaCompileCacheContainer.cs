using System;

namespace Framework.Core;

public class LambdaCompileCacheContainer : ILambdaCompileCacheContainer
{
    private readonly IDictionaryCache<Type, LambdaCompileCache> _cache1;

    private readonly IDictionaryCache<Tuple<Type, Type>, LambdaCompileCache> _cache2;

    private readonly IDictionaryCache<Tuple<object, Type>, LambdaCompileCache> _cache3;

    private readonly IDictionaryCache<Tuple<object, object>, LambdaCompileCache> _cache4;


    public LambdaCompileCacheContainer(LambdaCompileMode mode = LambdaCompileMode.None)
    {
        this._cache1 = new DictionaryCache<Type, LambdaCompileCache>(_ => new LambdaCompileCache(mode)).WithLock();
        this._cache2 = new DictionaryCache<Tuple<Type, Type>, LambdaCompileCache>(_ => new LambdaCompileCache(mode)).WithLock();
        this._cache3 = new DictionaryCache<Tuple<object, Type>, LambdaCompileCache>(_ => new LambdaCompileCache(mode)).WithLock();
        this._cache4 = new DictionaryCache<Tuple<object, object>, LambdaCompileCache>(_ => new LambdaCompileCache(mode)).WithLock();
    }


    public ILambdaCompileCache this[Type routeType1]
    {
        get
        {
            if (routeType1 == null) throw new ArgumentNullException(nameof(routeType1));

            return this._cache1[routeType1];
        }
    }

    public ILambdaCompileCache this[Type routeType1, Type routeType2]
    {
        get
        {
            if (routeType1 == null) throw new ArgumentNullException(nameof(routeType1));
            if (routeType2 == null) throw new ArgumentNullException(nameof(routeType2));

            return this._cache2.GetValue(routeType1, routeType2);
        }
    }

    public ILambdaCompileCache Get<TRouteType1, TRouteType2>()
    {
        return this[typeof(TRouteType1), typeof(TRouteType2)];
    }

    public ILambdaCompileCache Get<TRouteType1, TRouteType2>(TRouteType1 routeType1)
    {
        return this._cache3.GetValue(routeType1, typeof(TRouteType2));
    }

    public ILambdaCompileCache Get<TRouteType1, TRouteType2>(TRouteType1 routeType1, TRouteType2 routeType2)
    {
        return this._cache4.GetValue(routeType1, routeType2);
    }
}
