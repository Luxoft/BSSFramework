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
        Func<TDomainObject, SecurityAccessorResult> getAccessorsFunc = null,
        LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return securityProvider.And(SecurityProvider<TDomainObject>.Create(securityFilter, getAccessorsFunc, securityFilterCompileMode));
    }

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Expression<Func<TDomainObject, bool>> securityFilter,
        Func<TDomainObject, SecurityAccessorResult> getAccessorsFunc = null,
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
    public static ISecurityProvider<TDomainObject> Negate<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));

        return new NegateSecurityProvider<TDomainObject>(securityProvider);
    }

    public static ISecurityProvider<TDomainObject> Except<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider)
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (otherSecurityProvider == null) throw new ArgumentNullException(nameof(otherSecurityProvider));

        return securityProvider.And(otherSecurityProvider.Negate());
    }

    private class CompositeSecurityProvider<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider,
        bool orAnd)
        : ISecurityProvider<TDomainObject>
    {
        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            return orAnd
                       ? securityProvider.InjectFilter(queryable).Pipe(q => otherSecurityProvider.InjectFilter(q))
                       : securityProvider.InjectFilter(queryable).Concat(otherSecurityProvider.InjectFilter(queryable));
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            return orAnd
                       ? securityProvider.GetAccessResult(domainObject).And(otherSecurityProvider.GetAccessResult(domainObject))
                       : securityProvider.GetAccessResult(domainObject).Or(otherSecurityProvider.GetAccessResult(domainObject));
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return orAnd
                       ? securityProvider.HasAccess(domainObject) && otherSecurityProvider.HasAccess(domainObject)
                       : securityProvider.HasAccess(domainObject) || otherSecurityProvider.HasAccess(domainObject);
        }

        public SecurityAccessorResult GetAccessors(TDomainObject domainObject)
        {
            var left = securityProvider.GetAccessors(domainObject);

            var right = otherSecurityProvider.GetAccessors(domainObject);

            return orAnd
                       ? new SecurityAccessorResult.AndSecurityAccessorResult(left, right)
                       : new SecurityAccessorResult.OrSecurityAccessorResult(left, right);
        }
    }

    private class NegateSecurityProvider<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
        : ISecurityProvider<TDomainObject>
    {
        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            return queryable.Except(securityProvider.InjectFilter(queryable));
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            switch (securityProvider.GetAccessResult(domainObject))
            {
                case AccessResult.AccessDeniedResult accessResult:
                    return accessResult;

                case AccessResult.AccessGrantedResult:

                    switch (securityProvider.GetAccessResult(domainObject))
                    {
                        case AccessResult.AccessDeniedResult:
                            return AccessResult.AccessGrantedResult.Default;

                        case AccessResult.AccessGrantedResult:
                            return AccessResult.AccessDeniedResult.Create(domainObject);

                        default:
                            throw new InvalidOperationException("unknown access result");
                    }

                default:
                    throw new InvalidOperationException("unknown access result");
            }
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return !securityProvider.HasAccess(domainObject);
        }

        public SecurityAccessorResult GetAccessors(TDomainObject domainObject)
        {
            var baseResult = securityProvider.GetAccessors(domainObject);

            return new SecurityAccessorResult.NegateSecurityAccessorResult(baseResult);
        }
    }
}
