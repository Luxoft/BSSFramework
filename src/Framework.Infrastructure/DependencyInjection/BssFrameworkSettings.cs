using Framework.Application;
using Framework.Application.DependencyInjection;
using Framework.Application.Events;
using Framework.Database.DALListener;
using Framework.Database.ExpressionVisitorContainer;
using Framework.Infrastructure.Auth;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.DependencyInjection;

namespace Framework.Infrastructure.DependencyInjection;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = [];

    private readonly List<IBssFrameworkExtension> extensions = [];

    private Type domainObjectEventMetadataType = typeof(DomainObjectEventMetadata);

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemBuilder> setupAction)
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

    public IBssFrameworkSettings AddNamedLocks(Action<IGenericNamedLockBuilder> setupAction)
    {
        this.registerActions.Add(sc => sc.AddNamedLocks(setupAction));

        return this;
    }

    public IBssFrameworkSettings AddListener<TListener>()
        where TListener : class, IDALListener
    {
        this.registerActions.Add(sc => sc.RegisterListeners(s => s.Add<TListener>()));

        return this;
    }

    public IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension)
    {
        this.extensions.Add(extension);

        return this;
    }

    public IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.domainObjectEventMetadataType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings AddQueryVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
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
        this.InitializeDefault();

        services.AddScoped(typeof(IDomainObjectEventMetadata), this.domainObjectEventMetadataType);

        this.registerActions.ForEach(a => a(services));

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    private void InitializeDefault()
    {
        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }
    }
}
