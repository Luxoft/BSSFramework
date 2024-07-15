namespace Framework.SecuritySystem;

public static class SecurityProviderBaseExtensions
{
    public static void CheckAccess<TDomainObject>(
        this ISecurityProvider<TDomainObject> securityProvider,
        TDomainObject domainObject,
        IAccessDeniedExceptionService accessDeniedExceptionService)
        where TDomainObject : class
    {
        if (securityProvider == null) throw new ArgumentNullException(nameof(securityProvider));
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

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
