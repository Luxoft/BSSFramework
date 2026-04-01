using System.Data;

using CommonFramework.DependencyInjection;

using Framework.Core.LazyObject;
using Framework.Database._Visitors.Specific;
using Framework.Database.ConnectionStringSource;
using Framework.Database.DALExceptions;
using Framework.Database.ExpressionVisitorContainer;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public class DatabaseSetup : IDatabaseSetup, IServiceInitializer
{
    private DatabaseSettings settings = new();

    private readonly List<Action<IServiceCollection>> initActions = [];

    private string defaultConnectionStringName = "DefaultConnection";

    public bool AddDefaultListener { get; set; } = true;

    public IDatabaseSetup AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener
    {
        this.initActions.Add(services => services.AddScoped<IDBSessionEventListener, TEventListener>());

        return this;
    }

    public IDatabaseSetup SetIsolationLevel(IsolationLevel isolationLevel)
    {
        this.settings = this.settings with { IsolationLevel = isolationLevel };

        return this;
    }

    public IDatabaseSetup SetBatchSize(int batchSize)
    {
        this.settings = this.settings with { BatchSize = batchSize };

        return this;
    }

    public IDatabaseSetup SetCommandTimeout(int timeout)
    {
        this.settings = this.settings with { CommandTimeout = timeout };

        return this;
    }

    public IDatabaseSetup SetDefaultConnectionStringName(string connectionStringName)
    {
        this.defaultConnectionStringName = connectionStringName;

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();

        services.AddSingleton<IDalValidationIdentitySource, DalValidationIdentitySource>();

        services.AddScopedFrom<IDbTransaction, IDBSession>(session => session.Transaction);

        services.AddSingleton(DBSessionSettings.Default);

        services.AddScoped<IAuditPropertyFactory, AuditPropertyFactory>();

        services.AddSingleton<IInitializeManager, InitializeManager>();

        services.AddScoped<IDBSessionManager, DBSessionManager>();

        services.AddSingleton<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

        services.AddScoped<IPersistentInfoService, PersistentInfoService>();

        //For close db session by middleware
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);

        services.AddSingleton(this.settings);

        services.AddSingleton(new DefaultConnectionStringSettings(this.defaultConnectionStringName));

        if (this.AddDefaultListener)
        {
            this.AddEventListener<DefaultDBSessionEventListener>();
        }

        RegistryGenericDatabaseVisitors(services);

        foreach (var action in this.initActions)
        {
            action(services);
        }
    }

    private static IServiceCollection RegistryGenericDatabaseVisitors(IServiceCollection services)
    {
        //services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerMathItem>();

        services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

        return services;
    }
}
