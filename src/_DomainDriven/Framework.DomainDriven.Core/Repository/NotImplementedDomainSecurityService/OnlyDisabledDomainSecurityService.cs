using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.Repository.NotImplementedDomainSecurityService;

public class OnlyDisabledDomainSecurityService<TDomainObject> : OnlyDisabledDomainSecurityService<TDomainObject>, INotImplementedDomainSecurityService<TDomainObject>
    where TSecurityOperationCode : struct, Enum
{
    public OnlyDisabledDomainSecurityService(IDisabledSecurityProviderSource disabledSecurityProviderSource)
        : base(disabledSecurityProviderSource)
    {
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityOperation)
    {
        return this.GetSecurityProviderInternal(securityOperationCode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(SecurityOperation securityOperation)
    {
        return this.GetSecurityProviderInternal(securityOperation.Code);
    }
}

public class OnlyDisabledDomainSecurityService<TDomainObject> : INotImplementedDomainSecurityService<TDomainObject>
{
    private readonly IDisabledSecurityProviderSource disabledSecurityProviderSource;

    public OnlyDisabledDomainSecurityService(IDisabledSecurityProviderSource disabledSecurityProviderSource)
    {
        this.disabledSecurityProviderSource = disabledSecurityProviderSource;
    }

    public ISecurityProvider<TDomainObject> GetSecurityProvider(BLLSecurityMode securityMode)
    {
        return this.GetSecurityProviderInternal(securityMode);
    }

    public ISecurityProvider<TDomainObject> GetSecurityProviderInternal<TSecurityMode>(TSecurityMode securityMode)
    {
        if (!securityMode.IsDefault())
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }

        return this.disabledSecurityProviderSource.GetDisabledSecurityProvider<TDomainObject>();
    }
}
