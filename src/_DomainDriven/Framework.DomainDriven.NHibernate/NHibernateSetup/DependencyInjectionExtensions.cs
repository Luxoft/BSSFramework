using System;

using Framework.Core;
using Framework.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDatabaseSettings(this IServiceCollection services, Action<INHibernateSetupObject> setup)
    {
        var setupObject = new NHibernateSetupObject();

        services.AddScoped<INHibSessionSetup, NHibSessionSettings>();

        //For close db session by middleware
        services.AddScoped<INHibSessionSetup, NHibSessionSettings>();
        services.AddScopedFromLazyObject<INHibSession, NHibSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<INHibSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);

        services.AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>();
        services.AddSingleton<NHibConnectionSettings>();

        setup(setupObject);

        setupObject.SetEnvironmentAction(services);
        foreach (var action in setupObject.InitActions)
        {
            action(services);
        }

        return services;
    }
}
