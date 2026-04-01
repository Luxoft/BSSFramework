using CommonFramework.DependencyInjection;

using Framework.Core.LazyObject;
using Framework.Database.ExpressionVisitorContainer;
using Framework.Database.NHibernate.Sessions;
using Framework.DependencyInjection;

using GenericQueryable.NHibernate;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;

namespace Framework.Database.NHibernate.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNHibernate(this IServiceCollection services, Action<INHibernateSetupObject> setupAction)
    {
        var setupObject = new NHibernateSetupObject();

        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddNHibernateGenericQueryable();

        //For close db session by middleware
        services.AddScopedFromLazyObject<INHibSession, NHibSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<INHibSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);
        services.AddScoped<IDBSessionManager, DBSessionManager>();

        services.AddScopedFrom<ISession, INHibSession>(session => session.NativeSession);

        services.AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>();

        services.AddSingleton<NHibSessionEnvironment>();

        setupAction(setupObject);

        setupObject.Initialize(services);

        services.AddSingleton<IExpressionVisitorContainerItem, NHibExpressionVisitorContainerItem>();

        return services;
    }
}
