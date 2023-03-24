namespace Framework.SecuritySystem;

public class AccessDeniedSecurityProvider<TDomainObject> : ConstSecurityProvider<TDomainObject>
        where TDomainObject : class
{
    public AccessDeniedSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            : base(accessDeniedExceptionService, false)
    {
    }
}
