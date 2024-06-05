using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public static class SecurityProviderExtensions
{
    public static ISecurityProvider<TDomainObject> OverrideAccessDeniedResult<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Func<AccessResult.AccessDeniedResult, AccessResult.AccessDeniedResult> selector)
    {
        return new OverrideAccessDeniedResultSecurityProvider<TDomainObject>(securityProvider, selector);
    }

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Expression<Func<TDomainObject, bool>> securityFilter,
        Func<TDomainObject, UnboundedList<string>> getAccessorsFunc = null,
        LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return securityProvider.And(SecurityProvider<TDomainObject>.Create(securityFilter, getAccessorsFunc, securityFilterCompileMode));
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Expression<Func<TDomainObject, bool>> securityFilter,
        Func<TDomainObject, UnboundedList<string>> getAccessorsFunc = null,
        LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return securityProvider.Or(SecurityProvider<TDomainObject>.Create(securityFilter, getAccessorsFunc, securityFilterCompileMode));
    }

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (otherSecurityProvider == null) throw new ArgumentNullException(nameof(otherSecurityProvider));

        return new CompositeSecurityProvider<TDomainObject>(securityProvider, otherSecurityProvider, true);
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (otherSecurityProvider == null) throw new ArgumentNullException(nameof(otherSecurityProvider));

        return new CompositeSecurityProvider<TDomainObject>(securityProvider, otherSecurityProvider, false);
    }

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders)
    {
        if (securityProviders == null) throw new ArgumentNullException(nameof(securityProviders));

        return securityProviders.Match(
            () => new DisabledSecurityProvider<TDomainObject>(),
            single => single,
            many => many.Aggregate((v1, v2) => v1.And(v2)));
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders)
    {
        if (securityProviders == null) throw new ArgumentNullException(nameof(securityProviders));

        return securityProviders.Match(
            () => new AccessDeniedSecurityProvider<TDomainObject>(),
            single => single,
            many => many.Aggregate((v1, v2) => v1.Or(v2)));
    }

    private class CompositeSecurityProvider<TDomainObject> : ISecurityProvider<TDomainObject>
    {
        private readonly ISecurityProvider<TDomainObject> securityProvider;

        private readonly ISecurityProvider<TDomainObject> otherSecurityProvider;

        private readonly bool orAnd;


        public CompositeSecurityProvider(
            ISecurityProvider<TDomainObject> securityProvider,
            ISecurityProvider<TDomainObject> otherSecurityProvider,
            bool orAnd)
        {
            this.securityProvider = securityProvider;
            this.otherSecurityProvider = otherSecurityProvider;
            this.orAnd = orAnd;
        }


        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            return this.orAnd
                       ? this.securityProvider.InjectFilter(queryable).Pipe(q => this.otherSecurityProvider.InjectFilter(q))
                       : this.securityProvider.InjectFilter(queryable).Concat(this.otherSecurityProvider.InjectFilter(queryable));
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            return this.orAnd
                       ? this.securityProvider.GetAccessResult(domainObject).And(this.otherSecurityProvider.GetAccessResult(domainObject))
                       : this.securityProvider.GetAccessResult(domainObject).Or(this.otherSecurityProvider.GetAccessResult(domainObject));
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return this.orAnd
                       ? this.securityProvider.HasAccess(domainObject) && this.otherSecurityProvider.HasAccess(domainObject)
                       : this.securityProvider.HasAccess(domainObject) || this.otherSecurityProvider.HasAccess(domainObject);
        }

        public UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            var first = this.securityProvider.GetAccessors(domainObject);

            var second = this.otherSecurityProvider.GetAccessors(domainObject);

            var comparer = StringComparer.CurrentCultureIgnoreCase;

            return this.orAnd ? first.Union(second, comparer) : first.Concat(second).Distinct(comparer);
        }
    }
}
