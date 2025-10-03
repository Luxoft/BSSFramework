using CommonFramework;

using Framework.OData;

namespace Framework.DomainDriven;

public static class FetchServiceExtensions
{
    public static IFetchService<TPersistentDomainObjectBase, FetchBuildRule> Add<TPersistentDomainObjectBase>(
            this IFetchService<TPersistentDomainObjectBase, FetchBuildRule.DTOFetchBuildRule> mainService,
            IFetchService<TPersistentDomainObjectBase, SelectOperation> odataSerice)
    {
        if (mainService == null) throw new ArgumentNullException(nameof(mainService));
        if (odataSerice == null) throw new ArgumentNullException(nameof(odataSerice));

        return new MixedFetchService<TPersistentDomainObjectBase>(mainService, odataSerice);
    }

    public static IFetchService<TPersistentDomainObjectBase, TBuildRule> WithCompress<TPersistentDomainObjectBase, TBuildRule>(
            this IFetchService<TPersistentDomainObjectBase, TBuildRule> service)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        return new CompressFetchService<TPersistentDomainObjectBase, TBuildRule>(service);
    }


    public static IFetchService<TPersistentDomainObjectBase, TBuildRule> WithLock<TPersistentDomainObjectBase, TBuildRule>(
            this IFetchService<TPersistentDomainObjectBase, TBuildRule> service, object locker = null)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        return new LockFetchService<TPersistentDomainObjectBase, TBuildRule>(service, locker);
    }


    public static IFetchService<TPersistentDomainObjectBase, TBuildRule> WithCache<TPersistentDomainObjectBase, TBuildRule>(
            this IFetchService<TPersistentDomainObjectBase, TBuildRule> service)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));

        return new CacheFetchService<TPersistentDomainObjectBase, TBuildRule>(service);
    }




    private class CompressFetchService<TPersistentDomainObjectBase, TBuildRule> :
            IFetchService<TPersistentDomainObjectBase, TBuildRule>
    {
        private readonly IFetchService<TPersistentDomainObjectBase, TBuildRule> _baseService;

        public CompressFetchService(IFetchService<TPersistentDomainObjectBase, TBuildRule> baseService)
        {
            if (baseService == null) throw new ArgumentNullException(nameof(baseService));

            this._baseService = baseService;
        }

        public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
                where TDomainObject : TPersistentDomainObjectBase
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            return this._baseService.GetContainer<TDomainObject>(rule).Compress();
        }
    }

    private class LockFetchService<TPersistentDomainObjectBase, TBuildRule> :
            IFetchService<TPersistentDomainObjectBase, TBuildRule>
    {
        private readonly IFetchService<TPersistentDomainObjectBase, TBuildRule> _baseService;

        private readonly object _locker;

        public LockFetchService(IFetchService<TPersistentDomainObjectBase, TBuildRule> baseService, object locker)
        {
            if (baseService == null) throw new ArgumentNullException(nameof(baseService));

            this._baseService = baseService;
            this._locker = locker ?? new object();
        }

        public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
                where TDomainObject : TPersistentDomainObjectBase
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            lock (this._locker)
            {
                return this._baseService.GetContainer<TDomainObject>(rule);
            }
        }
    }

    private class CacheFetchService<TPersistentDomainObjectBase, TBuildRule> :
            IFetchService<TPersistentDomainObjectBase, TBuildRule>
    {
        private readonly IFetchService<TPersistentDomainObjectBase, TBuildRule> _baseService;

        private readonly Dictionary<Tuple<Type, TBuildRule>, object> _cache;


        public CacheFetchService(IFetchService<TPersistentDomainObjectBase, TBuildRule> baseService)
        {
            if (baseService == null) throw new ArgumentNullException(nameof(baseService));

            this._baseService = baseService;

            this._cache = new Dictionary<Tuple<Type, TBuildRule>, object>();

        }

        public IFetchContainer<TDomainObject> GetContainer<TDomainObject>(TBuildRule rule)
                where TDomainObject : TPersistentDomainObjectBase
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            return
                    (IFetchContainer<TDomainObject>)
                    this._cache.GetValueOrCreate(Tuple.Create(typeof(TDomainObject), rule), () => this._baseService.GetContainer<TDomainObject>(rule));
        }
    }
}
