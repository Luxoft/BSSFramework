namespace Framework.SecuritySystem
{
    public class DisabledSecurityProviderSource : IDisabledSecurityProviderSource
    {
        public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
        {
            return new DisabledSecurityProvider<TDomainObject>();
        }
    }
}
