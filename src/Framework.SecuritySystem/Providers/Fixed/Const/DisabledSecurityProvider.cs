namespace Framework.SecuritySystem
{
    public class DisabledSecurityProvider<TDomainObject> : ConstSecurityProvider<TDomainObject>
        where TDomainObject : class
    {
        public DisabledSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService)
            : base(accessDeniedExceptionService, true)
        {
        }
    }
}
