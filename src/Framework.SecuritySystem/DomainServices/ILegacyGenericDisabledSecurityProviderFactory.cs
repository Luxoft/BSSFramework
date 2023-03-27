namespace Framework.SecuritySystem;

public interface ILegacyGenericDisabledSecurityProviderFactory
{
    ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class;
}
