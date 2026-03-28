using CommonFramework.DependencyInjection;

using Framework.Application;
using Framework.Application.Auth;
using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Core.Auth;
using Framework.Database.DALListener;
using Framework.Database.ExpressionVisitorContainer;
using Framework.Infrastructure.Auth;
using Framework.Infrastructure.DALListener;
using Framework.Infrastructure.Integration;
using Framework.Infrastructure.Middleware;
using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class BssFrameworkBuilder : IBssFrameworkBuilder, IServiceInitializer
{
    private readonly List<Action<IServiceCollection>> registerActions = [];

    private readonly List<IBssFrameworkExtension> extensions = [];

    private Type domainObjectEventMetadataType = typeof(DomainObjectEventMetadata);

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public IBssFrameworkBuilder AddSecuritySystem(Action<ISecuritySystemBuilder> setupAction)
    {
        this.registerActions.Add(sc => sc.AddSecuritySystem(s =>
        {
            s.SetQueryableSource<DalQueryableSource>();
            s.SetGenericRepository<DalGenericRepository>();
            s.SetRawUserAuthenticationService<ApplicationUserAuthenticationService>();

            setupAction(s);
        }));

        return this;
    }

    public IBssFrameworkBuilder AddNamedLocks(Action<IGenericNamedLockBuilder> setupAction)
    {
        this.registerActions.Add(sc => sc.AddNamedLocks(setupAction));

        return this;
    }

    public IBssFrameworkBuilder AddListener<TListener>()
        where TListener : class, IDALListener
    {
        this.registerActions.Add(sc => sc.AddListeners(s => s.Add<TListener>()));

        return this;
    }

    public IBssFrameworkBuilder AddExtensions(IBssFrameworkExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }

    public IBssFrameworkBuilder SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.domainObjectEventMetadataType = typeof(T);

        return this;
    }

    public IBssFrameworkBuilder AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem
    {
        this.registerActions.Add(sc =>
        {
            if (scoped)
            {
                sc.AddScoped<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
            }
            else
            {
                sc.AddSingleton<IExpressionVisitorContainerItem, TExpressionVisitorContainerItem>();
            }
        });

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddGenericApplicationServices();

        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }

        services.AddSingleton(ApplicationDefaultUserAuthenticationServiceSettings.Default);
        services.AddSingleton<IDefaultUserAuthenticationService, ApplicationDefaultUserAuthenticationService>();

        services.AddSingleton<IEventXsdExporter2, EventXsdExporter2>();

        services.AddScoped<IWebApiExceptionExpander, WebApiExceptionExpander.WebApiExceptionExpander>();
        services.AddSingleton(WebApiExceptionExpanderSettings.Default);

        services.AddScoped<IWebApiCurrentDBSessionModeResolver, WebApiCurrentDBSessionModeResolver>();
        services.AddScoped<IWebApiCurrentMethodResolver, WebApiCurrentMethodResolver>();

        services.AddSingleton<IWebApiDBSessionModeResolver, WebApiDBSessionModeResolver>();

        services.AddScoped(typeof(IDomainObjectEventMetadata), this.domainObjectEventMetadataType);

        this.registerActions.ForEach(a => a(services));

        this.extensions.ForEach(ex => ex.AddServices(services));
    }
}
