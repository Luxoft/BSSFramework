using Anch.Core;
using Anch.DependencyInjection;

using Framework.Application;
using Framework.Application.Auth;
using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Core;
using Framework.Database.DALListener;
using Framework.Database.DependencyInjection;

using Framework.Infrastructure.Auth;
using Framework.Infrastructure.DALListener;
using Framework.Infrastructure.Integration;
using Framework.Infrastructure.Middleware;
using Framework.Infrastructure.Services;
using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class BssFrameworkSetup : IBssFrameworkSetup, IServiceInitializer
{
    private Action<IDatabaseSetup>? databaseSetupAction;

    private readonly List<Action<IServiceCollection>> registerActions = [];

    private readonly List<IBssFrameworkExtension> extensions = [];

    private Type domainObjectEventMetadataType = typeof(DomainObjectEventMetadata);

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;



    public IBssFrameworkSetup AddSecuritySystem(Action<ISecuritySystemSetup> setupAction)
    {
        this.registerActions.Add(services => services.AddSecuritySystem(s =>
        {
            s.SetQueryableSource<DalQueryableSource>();
            s.SetGenericRepository<DalGenericRepository>();
            s.SetDefaultCurrentUser<ApplicationDefaultCurrentUser>();
            s.SetDefaultCancellationTokenSource<ApplicationDefaultCancellationTokenSource>();

            setupAction(s);
        }));

        return this;
    }

    public IBssFrameworkSetup AddNamedLocks(Action<IGenericNamedLockSetup> setupAction)
    {
        this.registerActions.Add(services => services.AddNamedLocks(setupAction));

        return this;
    }

    public IBssFrameworkSetup AddDatabase(Action<IDatabaseSetup> setupAction)
    {
        this.databaseSetupAction = setupAction;

        return this;
    }

    public IBssFrameworkSetup AddListener<TListener>()
        where TListener : class, IDALListener
    {
        this.registerActions.Add(services => services.AddListeners(s => s.Add<TListener>()));

        return this;
    }

    public IBssFrameworkSetup AddExtensions(IBssFrameworkExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }

    public IBssFrameworkSetup SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.domainObjectEventMetadataType = typeof(T);

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IExceptionExpander, RootExceptionExpander>();

        services.AddSingleton<ICultureSource>(CultureSource.CurrentCulture);

        services.AddGenericApplicationServices();

        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }

        services.AddSingleton(ApplicationDefaultCurrentUserSettings.Default);

        services.AddSingleton<IEventXsdExporter2, EventXsdExporter2>();

        services.AddSingleton<IWebApiExceptionExpander, WebApiExceptionExpander.WebApiExceptionExpander>();
        services.AddSingleton(WebApiExceptionExpanderSettings.Default);

        services.AddScoped<IWebApiCurrentDBSessionModeResolver, WebApiCurrentDBSessionModeResolver>();
        services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

        services.AddSingleton<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();

        services.AddScoped(typeof(IDomainObjectEventMetadata), this.domainObjectEventMetadataType);

        services.AddGeneralDatabase(this.databaseSetupAction);

        this.registerActions.ForEach(a => a(services));

        this.extensions.ForEach(ex => ex.AddServices(services));
    }
}
