namespace Framework.SecuritySystem
{
    public class DisabledSecurityProviderContainer : IDisabledSecurityProviderSource
    {
        public DisabledSecurityProviderContainer()
        {
        }

        public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
        {
            return new DisabledSecurityProvider<TDomainObject>();
        }
    }
}
