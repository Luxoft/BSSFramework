using System.Linq.Expressions;

using Framework.Core;

namespace Framework.SecuritySystem;

public static class SecurityProviderExtensions
{
    public static void CheckAccess<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider, TDomainObject domainObject)
            where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        if (!securityProvider.HasAccess(domainObject))
        {
            throw securityProvider.GetAccessDeniedException(domainObject);
        }
    }

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider, Expression<Func<TDomainObject, bool>> securityFilter, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, Func<TDomainObject, UnboundedList<string>> getAccessorsFunc = null, LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)

            where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return securityProvider.And(SecurityProvider<TDomainObject>.Create(accessDeniedExceptionService, securityFilter, getAccessorsFunc, securityFilterCompileMode), accessDeniedExceptionService);
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider, Expression<Func<TDomainObject, bool>> securityFilter, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, Func<TDomainObject, UnboundedList<string>> getAccessorsFunc = null, LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)

            where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return securityProvider.Or(SecurityProvider<TDomainObject>.Create(accessDeniedExceptionService, securityFilter, getAccessorsFunc, securityFilterCompileMode), accessDeniedExceptionService);
    }



    public static ISecurityProvider<TDomainObject> And<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider, ISecurityProvider<TDomainObject> otherSecurityProvider, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (otherSecurityProvider == null) throw new ArgumentNullException(nameof(otherSecurityProvider));

        return new CompositeSecurityProvider<TDomainObject>(accessDeniedExceptionService, securityProvider, otherSecurityProvider, true);
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider, ISecurityProvider<TDomainObject> otherSecurityProvider, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (otherSecurityProvider == null) throw new ArgumentNullException(nameof(otherSecurityProvider));

        return new CompositeSecurityProvider<TDomainObject>(accessDeniedExceptionService, securityProvider, otherSecurityProvider, false);
    }

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)

            where TDomainObject : class
    {
        if (securityProviders == null) throw new ArgumentNullException(nameof(securityProviders));

        return securityProviders.Match(()     => new DisabledSecurityProvider<TDomainObject>(accessDeniedExceptionService),
                                       single => single,
                                       many   => many.Aggregate((v1, v2) => v1.And(v2, accessDeniedExceptionService)));
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders, IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)

            where TDomainObject : class
    {
        if (securityProviders == null) throw new ArgumentNullException(nameof(securityProviders));

        return securityProviders.Match(()     => new AccessDeniedSecurityProvider<TDomainObject>(accessDeniedExceptionService),
                                       single => single,
                                       many   => many.Aggregate((v1, v2) => v1.Or(v2, accessDeniedExceptionService)));
    }

    private class CompositeSecurityProvider<TDomainObject> : SecurityProviderBase<TDomainObject>
            where TDomainObject : class
    {
        private readonly ISecurityProvider<TDomainObject> securityProvider;

        private readonly ISecurityProvider<TDomainObject> otherSecurityProvider;

        private readonly bool orAnd;


        public CompositeSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, ISecurityProvider<TDomainObject> securityProvider, ISecurityProvider<TDomainObject> otherSecurityProvider, bool orAnd)
                : base(accessDeniedExceptionService)
        {
            this.securityProvider = securityProvider;
            this.otherSecurityProvider = otherSecurityProvider;
            this.orAnd = orAnd;
        }


        public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            return this.orAnd ? this.securityProvider.InjectFilter(queryable).Pipe(q => this.otherSecurityProvider.InjectFilter(q))
                           : this.securityProvider.InjectFilter(queryable).Concat(this.otherSecurityProvider.InjectFilter(queryable));
        }

        public override bool HasAccess(TDomainObject domainObject)
        {
            return this.orAnd ? this.securityProvider.HasAccess(domainObject) && this.otherSecurityProvider.HasAccess(domainObject)
                           : this.securityProvider.HasAccess(domainObject) || this.otherSecurityProvider.HasAccess(domainObject);
        }

        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            var first = this.securityProvider.GetAccessors(domainObject);

            var second = this.otherSecurityProvider.GetAccessors(domainObject);

            var comparer = StringComparer.CurrentCultureIgnoreCase;

            return this.orAnd ? first.Union(second, comparer) : first.Concat(second).Distinct(comparer);
        }
    }
}
