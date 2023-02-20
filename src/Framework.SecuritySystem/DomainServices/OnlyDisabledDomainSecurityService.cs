using System;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class OnlyDisabledDomainSecurityService<TDomainObject, TSecurityOperationCode> : IDomainSecurityService<TDomainObject, TSecurityOperationCode>
        where TSecurityOperationCode : struct, Enum where TDomainObject : class
{
    private readonly IServiceProvider serviceProvider;

    public OnlyDisabledDomainSecurityService(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
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

    public ISecurityProvider<TDomainObject> GetSecurityProviderInternal<T>(T securityMode)
    {
        if (!securityMode.IsDefault())
        {
            throw new InvalidOperationException($"Security mode \"{securityMode}\" not allowed");
        }

        return ActivatorUtilities.CreateInstance<DisabledSecurityProvider<TDomainObject>>(this.serviceProvider);
    }
}
