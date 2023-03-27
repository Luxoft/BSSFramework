using Framework.Core;

namespace Framework.SecuritySystem;

public class OnlyDisabledDomainSecurityService<TDomainObject, TSecurityOperationCode> : IDomainSecurityService<TDomainObject, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum where TDomainObject : class
{
    private readonly ILegacyGenericDisabledSecurityProviderFactory legacyGenericDisabledSecurityProviderFactory;

    public OnlyDisabledDomainSecurityService(ILegacyGenericDisabledSecurityProviderFactory legacyGenericDisabledSecurityProviderFactory)
    {
        this.legacyGenericDisabledSecurityProviderFactory = legacyGenericDisabledSecurityProviderFactory;
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.GetSecurityProviderInternal(securityMode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode)
    {
        return this.GetSecurityProviderInternal(securityOperationCode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation)
    {
        return this.GetSecurityProviderInternal(securityOperation.Code);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProviderInternal<TSecurityMode>(TSecurityMode securityMode)
    {
        if (!securityMode.IsDefault())
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }

        return this.legacyGenericDisabledSecurityProviderFactory.GetDisabledSecurityProvider<TDomainObject>();
    }
}
