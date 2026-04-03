using System.Data;
using System.Linq.Expressions;

using CommonFramework.DependencyInjection;

using Framework.Core.LazyObject;
using Framework.Database._Visitors.Containers;
using Framework.Database.ConnectionStringSource;
using Framework.Database.DALExceptions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Database.DependencyInjection;

public class DatabaseSetup : IDatabaseSetup, IServiceInitializer
{
    private DatabaseSettings settings = new();

    private readonly List<Action<IServiceCollection>> initActions = [];

    private string? defaultConnectionString;

    private string defaultConnectionStringName = "DefaultConnection";

    public bool AddDefaultListener { get; set; } = true;

    public IDatabaseSetup AddEventListener<TEventListener>()
        where TEventListener : class, IDBSessionEventListener
    {
        this.initActions.Add(services => services.AddScoped<IDBSessionEventListener, TEventListener>());

        return this;
    }

    public IDatabaseSetup AddVisitorContainer<TExpressionVisitorContainer>()
        where TExpressionVisitorContainer : class, IExpressionVisitorContainer
    {
        this.initActions.Add(sc => sc.AddKeyedSingleton<IExpressionVisitorContainer, TExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey));

        return this;
    }

    public IDatabaseSetup AddVisitor(ExpressionVisitor expressionVisitor)
    {
        this.initActions.Add(sc => sc.AddKeyedSingleton<IExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey, new ExpressionVisitorContainer(expressionVisitor)));

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

    public IDatabaseSetup SetDefaultConnectionString(string connectionString)
    {
        this.defaultConnectionString = connectionString;

        return this;
    }

    public IDatabaseSetup SetDefaultConnectionStringName(string connectionStringName)
    {
        this.defaultConnectionStringName = connectionStringName;

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IDalValidationIdentitySource, DalValidationIdentitySource>();

        services.AddScopedFrom<IDbTransaction, IDBSession>(session => session.Transaction);

        services.AddSingleton(DBSessionSettings.Default);

        services.AddScoped<IAuditPropertyFactory, AuditPropertyFactory>();

        services.AddSingleton<IInitializeManager, InitializeManager>();

        services.AddScoped<IDBSessionManager, DBSessionManager>();

        services.AddScoped<IPersistentInfoService, PersistentInfoService>();

        //For close db session by middleware
        services.AddScopedFrom((ILazyObject<IDBSession> lazyDbSession) => lazyDbSession.Value);

        services.AddSingleton(this.settings);

        if (this.defaultConnectionString != null)
        {
            services.AddSingleton<IDefaultConnectionStringSource>(new ManualDefaultConnectionStringSource(this.defaultConnectionString));
        }
        else
        {
            services.AddSingleton(new DefaultConnectionStringSettings(this.defaultConnectionStringName));
            services.AddSingleton<IDefaultConnectionStringSource, DefaultConnectionStringSource>();
        }

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
        services.AddSingleton<IExpressionVisitorContainer, RootExpressionVisitorContainer>();

        //services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddKeyedSingleton<IExpressionVisitorContainer, PeriodExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey);
        services.AddKeyedSingleton<IExpressionVisitorContainer, DefaultExpressionVisitorContainer>(IExpressionVisitorContainer.ElementKey);
        services.AddKeyedSingleton<IExpressionVisitorContainer, OverrideEqualsDomainObjectVisitorContainer>(IExpressionVisitorContainer.ElementKey);

        return services;
    }
}
