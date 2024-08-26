using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Authorization.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;
using Framework.SecuritySystem.DependencyInjection;
using Framework.DomainDriven._Visitors;
using Framework.DomainDriven.NHibernate;
using Framework.SecuritySystem;

using nuSpec.Abstraction;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Authorization.SecuritySystem.Validation;

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Framework.DomainDriven.Setup;

public class BssFrameworkSettings : IBssFrameworkSettings
{
    public List<Type> NamedLockTypes { get; set; } = new();

    public bool RegisterBaseNamedLockTypes { get; set; } = true;

    public bool RegisterDenormalizeHierarchicalDALListener { get; set; } = true;

    public List<Action<IServiceCollection>> RegisterActions { get; set; } = new();

    public List<IBssFrameworkExtension> Extensions = new();

    public Type NotificationPrincipalExtractorType { get; private set; } = typeof(NotificationPrincipalExtractor);

    public Type DomainObjectEventMetadataType { get; private set; } = typeof(DomainObjectEventMetadata);

    public Type? SpecificationEvaluatorType { get; private set; }

    public DomainSecurityRule.RoleBaseSecurityRule SecurityAdministratorRule { get; private set; } = SecurityRole.Administrator;

    public IBssFrameworkSettings AddSecuritySystem(Action<ISecuritySystemSettings> setupAction)
    {
        this.RegisterActions.Add(
            sc => sc.AddSecuritySystem(
                sss =>
                {
                    sss.SetCurrentUser<AuthorizationCurrentUser>();

                    setupAction(sss);
                }));

        return this;
    }

    public IBssFrameworkSettings AddNamedLockType(Type namedLockType)
    {
        this.NamedLockTypes.Add(namedLockType);

        return this;
    }

    public IBssFrameworkSettings AddListener<TListener>(bool registerSelf = false)
        where TListener : class, IDALListener
    {
        this.RegisterActions.Add(sc => sc.RegisterListeners(s => s.Add<TListener>(registerSelf)));

        return this;
    }

    public IBssFrameworkSettings AddExtensions(IBssFrameworkExtension extension)
    {
        this.Extensions.Add(extension);

        return this;
    }

    public IBssFrameworkSettings SetNotificationPrincipalExtractor<T>()
        where T : INotificationPrincipalExtractor
    {
        this.NotificationPrincipalExtractorType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings SetDomainObjectEventMetadata<T>()
        where T : IDomainObjectEventMetadata
    {
        this.DomainObjectEventMetadataType = typeof(T);

        return this;
    }

    public IBssFrameworkSettings SetSecurityAdministratorRule(DomainSecurityRule.RoleBaseSecurityRule rule)
    {
        this.SecurityAdministratorRule = rule;

        return this;
    }

    public IBssFrameworkSettings SetSpecificationEvaluator<TSpecificationEvaluator>()
        where TSpecificationEvaluator : class, ISpecificationEvaluator
    {
        this.SpecificationEvaluatorType = typeof(TSpecificationEvaluator);

        return this;
    }

    public IBssFrameworkSettings SetUniquePermissionValidator<TValidator>()
        where TValidator : class, IValidator<Principal>
    {
        this.RegisterActions.Add(
            sc => sc.Replace(ServiceDescriptor.KeyedScoped<IValidator<Principal>, TValidator>(PrincipalUniquePermissionValidator.Key)));

        return this;
    }

    public IBssFrameworkSettings AddDatabaseVisitors<TExpressionVisitorContainerItem>(bool scoped = false)
        where TExpressionVisitorContainerItem : class, IExpressionVisitorContainerItem
    {
        this.RegisterActions.Add(
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
        this.RegisterActions.Add(sc => sc.AddDatabaseSettings(setup));

        return this;
    }

    public void TryInitDefault()
    {
        if (this.RegisterBaseNamedLockTypes)
        {
            this.NamedLockTypes.Add(typeof(ConfigurationNamedLock));
        }

        if (this.RegisterDenormalizeHierarchicalDALListener)
        {
            this.AddListener<DenormalizeHierarchicalDALListener>();
        }
    }

    public void Init()
    {
        if (this.SpecificationEvaluatorType == null)
        {
            this.RegisterActions.Add(sc => sc.AddSingleton(LazyInterfaceImplementHelper.CreateNotImplemented<ISpecificationEvaluator>()));
        }
        else
        {
            this.RegisterActions.Add(sc => sc.AddScoped(typeof(ISpecificationEvaluator), this.SpecificationEvaluatorType));
        }
    }
}
