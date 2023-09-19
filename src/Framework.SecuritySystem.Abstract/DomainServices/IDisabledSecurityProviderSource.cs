namespace Framework.SecuritySystem;

public interface IDisabledSecurityProviderSource
{
    ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>();
}
