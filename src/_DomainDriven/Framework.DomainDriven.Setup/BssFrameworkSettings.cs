using Microsoft.Extensions.DependencyInjection;

using Framework.DomainDriven.ApplicationCore.Security;
using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.NHibernate;
using Framework.SecuritySystem;
using Framework.DependencyInjection;
using Framework.DomainDriven.ApplicationCore;
using Framework.DomainDriven.ApplicationCore.DALListeners;
using Framework.DomainDriven.Lock;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    private readonly List<Action<IServiceCollection>> registerActions = new();

    private readonly List<IBssFrameworkExtension> extensions = new();

    private Type domainObjectEventMetadataType = typeof(DomainObjectEventMetadata);

    private Type? specificationEvaluatorType;

    private DomainSecurityRule.RoleBaseSecurityRule securityAdministratorRule = SecurityRole.Administrator;

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction)
    {
        this.registerActions.Add(sc => sc.AddSecuritySystem(setupAction));

        return this;
    }

    public IBssFrameworkSettings AddNamedLocks(Action<IGenericNamedLockSetup> setupAction)
    {
        this.registerActions.Add(sc => sc.RegisterNamedLocks(setupAction));

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

    public IBssFrameworkSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule)
    {
        this.securityAdministratorRule = rule;

        return this;
    }

    public IBssFrameworkSettings SetSpecificationEvaluator<TSpecificationEvaluator>()
        where TSpecificationEvaluator : class, ISpecificationEvaluator
    {
        this.specificationEvaluatorType = typeof(TSpecificationEvaluator);

        return this;
    }

    public IBssFrameworkSettings AddDatabaseVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem
    {
        this.registerActions.Add(
            sc =>
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

    public IBssFrameworkSettings AddDatabaseSettings(Action<INHibernateSetupObject> setup)
    {
        this.registerActions.Add(sc => sc.AddDatabaseSettings(setup));

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        this.InitializeDefault();

        services.AddSingleton(new SecurityAdministratorRuleInfo(this.securityAdministratorRule));

        services.AddScoped(typeof(IDomainObjectEventMetadata), this.domainObjectEventMetadataType);

        this.registerActions.ForEach(a => a(services));

        this.extensions.ForEach(ex => ex.AddServices(services));
    }

    private void InitializeDefault()
    {
        this.registerActions.Add(
            sc =>
            {
                if (this.specificationEvaluatorType == null)
                {
                    sc.AddNotImplemented<ISpecificationEvaluator>("Use 'SetSpecificationEvaluator'");
                }
                else
                {
                    sc.AddScoped(typeof(ISpecificationEvaluator), this.specificationEvaluatorType);
                }
            });

        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }
    }
}
