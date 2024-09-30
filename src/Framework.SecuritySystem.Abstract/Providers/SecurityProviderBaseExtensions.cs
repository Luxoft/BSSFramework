using Framework.Core;
using System.Linq.Expressions;

namespace Framework.SecuritySystem;

public static class SecurityProviderBaseExtensions
{
    public static ISecurityProvider<TDomainObject> OverrideAccessDeniedResult<TDomainObject>(
            this ISecurityProvider<TDomainObject> securityProvider,
            Func<AccessResult.AccessDeniedResult, AccessResult.AccessDeniedResult> selector) =>
            new OverrideAccessDeniedResultSecurityProvider<TDomainObject>(securityProvider, selector);

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Expression<Func<TDomainObject, bool>> securityFilter,
        LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All) =>
        securityProvider.And(SecurityProvider<TDomainObject>.Create(securityFilter, securityFilterCompileMode));

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        Expression<Func<TDomainObject, bool>> securityFilter,
        LambdaCompileMode securityFilterCompileMode = LambdaCompileMode.All) =>
        securityProvider.Or(SecurityProvider<TDomainObject>.Create(securityFilter, securityFilterCompileMode));

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider) =>
        new CompositeSecurityProvider<TDomainObject>(securityProvider, otherSecurityProvider, true);

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider) =>
        new CompositeSecurityProvider<TDomainObject>(securityProvider, otherSecurityProvider, false);

    public static ISecurityProvider<TDomainObject> And<TDomainObject>(
        this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders) =>
        securityProviders.Match(
            () => new DisabledSecurityProvider<TDomainObject>(),
            single => single,
            many => many.Aggregate((v1, v2) => v1.And(v2)));

    public static ISecurityProvider<TDomainObject> Or<TDomainObject>(
        this IEnumerable<ISecurityProvider<TDomainObject>> securityProviders) =>
        securityProviders.Match(
            () => new AccessDeniedSecurityProvider<TDomainObject>(),
            single => single,
            many => many.Aggregate((v1, v2) => v1.Or(v2)));

    public static ISecurityProvider<TDomainObject> Negate<TDomainObject>(this ISecurityProvider<TDomainObject> securityProvider) =>
        new NegateSecurityProvider<TDomainObject>(securityProvider);

    public static ISecurityProvider<TDomainObject> Except<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider) =>
        securityProvider.And(otherSecurityProvider.Negate());

    private class CompositeSecurityProvider<TDomainObject>(
        ISecurityProvider<TDomainObject> securityProvider,
        ISecurityProvider<TDomainObject> otherSecurityProvider,
        bool orAnd)
        : ISecurityProvider<TDomainObject>
    {
        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) =>
            orAnd
                ? securityProvider.InjectFilter(queryable).Pipe(otherSecurityProvider.InjectFilter)
                : securityProvider.InjectFilter(queryable).Union(otherSecurityProvider.InjectFilter(queryable));

        public AccessResult GetAccessResult(TDomainObject domainObject) =>
            orAnd
                ? securityProvider.GetAccessResult(domainObject).And(otherSecurityProvider.GetAccessResult(domainObject))
                : securityProvider.GetAccessResult(domainObject).Or(otherSecurityProvider.GetAccessResult(domainObject));

        public bool HasAccess(TDomainObject domainObject) =>
            orAnd
                ? securityProvider.HasAccess(domainObject) && otherSecurityProvider.HasAccess(domainObject)
                : securityProvider.HasAccess(domainObject) || otherSecurityProvider.HasAccess(domainObject);

        public SecurityAccessorData GetAccessorData(TDomainObject domainObject)
        {
            var left = securityProvider.GetAccessorData(domainObject);

            var right = otherSecurityProvider.GetAccessorData(domainObject);

            return orAnd
                       ? new SecurityAccessorData.AndSecurityAccessorData(left, right)
                       : new SecurityAccessorData.OrSecurityAccessorData(left, right);
        }
    }

    private class NegateSecurityProvider<TDomainObject>(ISecurityProvider<TDomainObject> securityProvider)
        : ISecurityProvider<TDomainObject>
    {
        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable) =>
            queryable.Except(securityProvider.InjectFilter(queryable));

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            switch (securityProvider.GetAccessResult(domainObject))
            {
                case AccessResult.AccessDeniedResult:
                    return AccessResult.AccessGrantedResult.Default;

                case AccessResult.AccessGrantedResult:
                    return AccessResult.AccessDeniedResult.Create(domainObject);

                default:
                    throw new InvalidOperationException("unknown access result");
            }
        }

        public bool HasAccess(TDomainObject domainObject) => !securityProvider.HasAccess(domainObject);

        public SecurityAccessorData GetAccessorData(TDomainObject domainObject)
        {
            var baseResult = securityProvider.GetAccessorData(domainObject);

            return new SecurityAccessorData.NegateSecurityAccessorData(baseResult);
        }
    }

    public static void CheckAccess<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        TDomainObject domainObject,
        IAccessDeniedExceptionService accessDeniedExceptionService)
        where TDomainObject : class
    {
        switch (securityProvider.GetAccessResult(domainObject))
        {
            case AccessResult.AccessDeniedResult accessDenied:
                throw accessDeniedExceptionService.GetAccessDeniedException(accessDenied);

            case AccessResult.AccessGrantedResult:
                break;

            default:
                throw new InvalidOperationException("unknown access result");
        }
    }
}
