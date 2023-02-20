using System;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class OnlyDisabledDomainSecurityServiceContainer : INotImplementedDomainSecurityServiceContainer
{
    private readonly IServiceProvider serviceProvider;

    public OnlyDisabledDomainSecurityServiceContainer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public IDomainSecurityService<TDomainObject, TSecurityOperationCode> GetNotImplementedDomainSecurityService<TDomainObject, TSecurityOperationCode>()
            where TDomainObject : class
            where TSecurityOperationCode : struct, Enum
    {
        return ActivatorUtilities.CreateInstance<OnlyDisabledDomainSecurityService<TDomainObject, TSecurityOperationCode>>(this.serviceProvider);
    }
}
