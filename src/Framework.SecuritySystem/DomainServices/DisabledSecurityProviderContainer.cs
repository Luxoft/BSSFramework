namespace Framework.SecuritySystem
{
    public class DisabledSecurityProviderContainer<TPersistentDomainObjectBase> : IDisabledSecurityProviderContainer<TPersistentDomainObjectBase>
    {
        public DisabledSecurityProviderContainer()
        {
        }

        public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            return new DisabledSecurityProvider<TDomainObject>();
        }
    }
}
