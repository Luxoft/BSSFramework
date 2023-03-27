using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public abstract class SecurityProvider<TDomainObject> : SecurityProviderBase<TDomainObject>
        where TDomainObject : class
{
    private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;


    protected SecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            : base(accessDeniedExceptionService)
    {
        this.lazyHasAccessFunc = LazyHelper.Create(() => this.SecurityFilter.Compile(this.CompileCache));
    }


    protected LambdaCompileCache CompileCache
    {
        get { return Caches[this.SecurityFilterCompileMode]; }
    }

    public abstract Expression<Func<TDomainObject, bool>> SecurityFilter { get; }

    protected virtual LambdaCompileMode SecurityFilterCompileMode
    {
        get { return LambdaCompileMode.None; }
    }


    public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
    {
        if (queryable == null) throw new ArgumentNullException(nameof(queryable));

        return queryable.Where(this.SecurityFilter);
    }

    public override bool HasAccess(TDomainObject domainObject)
    {
        return this.lazyHasAccessFunc.Value(domainObject);
    }

    private static readonly IDictionaryCache<LambdaCompileMode, LambdaCompileCache> Caches =
            new DictionaryCache<LambdaCompileMode, LambdaCompileCache>(mode => new LambdaCompileCache(mode)).WithLock();


    public static SecurityProvider<TDomainObject> Create(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, Expression<Func<TDomainObject, bool>> securityFilter, Func<TDomainObject, UnboundedList<string>> getAccessorsFunc = null, LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)
    {
        return new DefaultSecurityProvider(accessDeniedExceptionService, securityFilter, getAccessorsFunc, securityFilterCompileMode);
    }

    private class DefaultSecurityProvider : SecurityProvider<TDomainObject>
    {
        private readonly Expression<Func<TDomainObject, bool>> securityFilter;
        private readonly Func<TDomainObject, UnboundedList<string>> getAccessorsFunc;
        private readonly LambdaCompileMode securityFilterCompileMode;

        public DefaultSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, Expression<Func<TDomainObject, bool>> securityFilter, Func<TDomainObject, UnboundedList<string>> getAccessorsFunc, LambdaCompileMode securityFilterCompileMode)
                : base(accessDeniedExceptionService)
        {
            this.securityFilter = securityFilter ?? throw new ArgumentNullException(nameof(securityFilter));
            this.getAccessorsFunc = getAccessorsFunc ?? (domainObject => this.HasAccess(domainObject) ? UnboundedList<string>.Infinity : UnboundedList<string>.Empty);
            this.securityFilterCompileMode = securityFilterCompileMode;
        }


        public override Expression<Func<TDomainObject, bool>> SecurityFilter
        {
            get { return this.securityFilter; }
        }

        protected override LambdaCompileMode SecurityFilterCompileMode
        {
            get { return this.securityFilterCompileMode; }
        }

        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return this.getAccessorsFunc(domainObject);
        }
    }
}
