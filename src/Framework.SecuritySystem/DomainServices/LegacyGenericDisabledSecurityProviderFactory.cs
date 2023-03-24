using System;
using System.Linq;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.SecuritySystem;

public class LegacyGenericDisabledSecurityProviderFactory : ILegacyGenericDisabledSecurityProviderFactory
{
    private readonly IServiceProvider serviceProvider;

    private readonly LegacyPersistentDomainObjectBaseList persistentDomainObjectBaseList;

    public LegacyGenericDisabledSecurityProviderFactory(IServiceProvider serviceProvider, LegacyPersistentDomainObjectBaseList persistentDomainObjectBaseList)
    {
        this.serviceProvider = serviceProvider;
        this.persistentDomainObjectBaseList = persistentDomainObjectBaseList;
    }

    public ISecurityProvider<TDomainObject> GetDisabledSecurityProvider<TDomainObject>()
            where TDomainObject : class
    {
        var persistentDomainObjectBase = this.persistentDomainObjectBaseList.Single(t => t.IsAssignableFrom(typeof(TDomainObject)));

        var accessDeniedExceptionServiceType = typeof(IAccessDeniedExceptionService<>).MakeGenericType(persistentDomainObjectBase);

        var accessDeniedExceptionService = this.serviceProvider.GetRequiredService(accessDeniedExceptionServiceType);

        return ActivatorUtilities.CreateInstance<DisabledSecurityProvider<TDomainObject>>(this.serviceProvider, accessDeniedExceptionService);
    }
}
