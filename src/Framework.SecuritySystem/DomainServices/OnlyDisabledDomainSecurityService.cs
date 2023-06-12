using Framework.Core;

namespace Framework.SecuritySystem;

public class OnlyDisabledDomainSecurityService<TDomainObject, TSecurityOperationCode> : OnlyDisabledDomainSecurityService<TDomainObject>, IDomainSecurityService<TDomainObject, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum
        where TDomainObject : class
{
    public OnlyDisabledDomainSecurityService(ILegacyGenericDisabledSecurityProviderFactory legacyGenericDisabledSecurityProviderFactory)
        :base(legacyGenericDisabledSecurityProviderFactory)
    {
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(TSecurityOperationCode securityOperationCode)
    {
        return this.GetSecurityProviderInternal(securityOperationCode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation<TSecurityOperationCode> securityOperation)
    {
        return this.GetSecurityProviderInternal(securityOperation.Code);
    }
}

public class OnlyDisabledDomainSecurityService<TDomainObject> : IDomainSecurityService<TDomainObject>
    where TDomainObject : class
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

    protected ISecurityProvider<TDomainObject> GetSecurityProviderInternal<TSecurityMode>(TSecurityMode securityMode)
    {
        if (!securityMode.IsDefault())
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }

        return this.legacyGenericDisabledSecurityProviderFactory.GetDisabledSecurityProvider<TDomainObject>();
    }
}
