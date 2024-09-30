using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem
{
    public abstract class SecurityProvider<TDomainObject> : ISecurityProvider<TDomainObject>
    {
        private readonly Lazy<Func<TDomainObject, bool>> lazyHasAccessFunc;


        protected SecurityProvider() => this.lazyHasAccessFunc = LazyHelper.Create(() => this.SecurityFilter.Compile(this.CompileCache));

        protected LambdaCompileCache CompileCache => Caches[this.SecurityFilterCompileMode];

        public abstract Expression<Func<TDomainObject, bool>> SecurityFilter { get; }

        protected virtual LambdaCompileMode SecurityFilterCompileMode { get; } = LambdaCompileMode.All;

        public virtual IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) => queryable.Where(this.SecurityFilter);

        public virtual bool HasAccess(TDomainObject domainObject) => this.lazyHasAccessFunc.Value(domainObject);

        public abstract SecurityAccessorData GetAccessorData(TDomainObject domainObject);

        private static readonly IDictionaryCache<LambdaCompileMode, LambdaCompileCache> Caches =
            new DictionaryCache<LambdaCompileMode, LambdaCompileCache>(mode => new LambdaCompileCache(mode)).WithLock();


        public static ISecurityProvider<TDomainObject> Create(
            Expression<Func<TDomainObject, bool>> securityFilter,
            LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All) =>
            new ConditionSecurityProvider<TDomainObject>(securityFilter, securityFilterCompileMode);
    }
}
