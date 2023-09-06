namespace Framework.SecuritySystem
{
    public class AccessDeniedSecurityProvider<TDomainObject> : ConstSecurityProvider<TDomainObject>
    {
        public AccessDeniedSecurityProvider()
            : base(false)
        {
        }
    }
}
