using System.Data;

using CommonFramework.DependencyInjection;

using Framework.Core.LazyObject;
using Framework.Database.ConnectionStringSource;
using Framework.Database.DALExceptions;
using Framework.Database.ExpressionVisitorContainer;
using Framework.Database.NHibernate.Audit;
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

        services.AddSingleton(DBSessionSettings.Default);

        services.AddScoped<IAuditPropertyFactory, AuditPropertyFactory>();

        services.AddNHibernateGenericQueryable();

        //For close db session by middleware
        services.AddScopedFromLazyObject<INHibSession, NHibSession>();
        services.AddScopedFrom<ILazyObject<IDBSession>, ILazyObject<INHibSession>>();
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);
        services.AddScoped<IDBSessionManager, DBSessionManager>();

        services.AddScopedFrom<ISession, INHibSession>(session => session.NativeSession);
        services.AddScopedFrom<IDbTransaction, IDBSession>(session => session.Transaction);

        services.AddSingleton<INHibSessionEnvironmentSettings, NHibSessionEnvironmentSettings>();

        services.AddSingleton<IDalValidationIdentitySource, DalValidationIdentitySource>();

        services.AddSingleton<NHibSessionEnvironment>();

        services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        setupAction(setupObject);

        setupObject.Initialize(services);

        services.AddSingleton<IExpressionVisitorContainerItem, NHibExpressionVisitorContainerItem>();

        return services;
    }
}
