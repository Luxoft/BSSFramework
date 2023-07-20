using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.NHibernate;
using Framework.DomainDriven.Repository;
using Framework.HierarchicalExpand;
using Framework.Notification;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IOperationEventSenderContainer<>), typeof(OperationEventSenderContainer<>));

        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));
        services.AddScoped(typeof(IAsyncDal<,>), typeof(NHibAsyncDal<,>));

        services.AddScoped(typeof(IRepositoryFactory<>), typeof(RepositoryFactory<>));
        services.AddScoped(typeof(IRepositoryFactory<,>), typeof(RepositoryFactory<,>));
        services.AddScoped(typeof(IGenericRepositoryFactory<,,>), typeof(GenericRepositoryFactory<,,>));

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

        services.AddScopedFrom((IDBSession session) => session.GetObjectStateService());

        services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

        services.AddSingleton<IDBSessionEvaluator, DBSessionEvaluator>();

        services.RegisterAuthorizationSystem();

        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();

        services.AddScoped<ILegacyGenericDisabledSecurityProviderFactory, LegacyGenericDisabledSecurityProviderFactory>();
        services.AddScoped<INotImplementedDomainSecurityServiceContainer, OnlyDisabledDomainSecurityServiceContainer>();

        return services;
    }

    public static IServiceCollection RegistryGenericDatabaseVisitors(this IServiceCollection services)
    {
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPersistentItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerPeriodItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDefaultItem>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerMathItem>();

        services.AddSingleton<IIdPropertyResolver, IdPropertyResolver>();

        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>();
        services.AddSingleton<IExpressionVisitorContainerItem, ExpressionVisitorContainerDomainIdentItem<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>();

        services.AddScoped<IExpressionVisitorContainer, ExpressionVisitorAggregator>();

        return services;
    }

    public static IServiceCollection RegisterHierarchicalObjectExpander<TPersistentDomainObjectBase>(this IServiceCollection services)
            where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        return services.AddSingleton<IHierarchicalRealTypeResolver, ProjectionHierarchicalRealTypeResolver>()
                       .AddScoped<IHierarchicalObjectExpanderFactory<Guid>, HierarchicalObjectExpanderFactory<TPersistentDomainObjectBase, Guid>>();
    }

    private static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScopedFrom<IAuthorizationSystem<Guid>, IAuthorizationBLLContext>();
    }

    public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<AuthorizationValidatorCompileCache>()
               .AddScoped<IAuthorizationValidator, AuthorizationValidator>()

               .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
               .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
               .AddScoped<IRunAsManager, AuthorizationRunAsManger>()
               .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()
               .AddScoped<INotificationPrincipalExtractor, LegacyNotificationPrincipalExtractor>()
               .AddScoped<INotificationBasePermissionFilterSource, LegacyNotificationPrincipalExtractor>()
               .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()

               .AddScopedFromLazyInterfaceImplement<IAuthorizationBLLContext, AuthorizationBLLContext>()

               .AddScopedFrom<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.AuthorizationSecurityOperationCode>, IAuthorizationBLLContext>()
               .AddScopedFrom<IDisabledSecurityProviderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>, IAuthorizationSecurityService>()
               .AddScopedFrom<IAuthorizationSecurityPathContainer, IAuthorizationSecurityService>()
               .AddScoped<IQueryableSource<Framework.Authorization.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.DomainObjectBase, Guid>>()
               .AddScoped<ISecurityExpressionBuilderFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()
               .AddScoped<IAccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()

               .Self(AuthorizationSecurityServiceBase.Register)
               .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<ConfigurationValidatorCompileCache>()
               .AddScoped<IConfigurationValidator, ConfigurationValidator>()

               .AddSingleton(new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IConfigurationSecurityService, ConfigurationSecurityService>()
               .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

               .AddScopedFrom<ICurrentRevisionService, IDBSession>()

               .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
               .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()

               .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
               .AddScopedFromLazyInterfaceImplement<IConfigurationBLLContext, ConfigurationBLLContext>()

               .AddScopedFrom<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext, IConfigurationBLLContext>()

               .AddScopedFrom<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>, IConfigurationBLLContext>()
               .AddScopedFrom<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>, IConfigurationSecurityService>()
               .AddScopedFrom<IConfigurationSecurityPathContainer, IConfigurationSecurityService>()
               .AddScoped<IQueryableSource<Framework.Configuration.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectBase, Guid>>()
               .AddScoped<ISecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
               .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()

               .Self(ConfigurationSecurityServiceBase.Register)
               .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
    }
}
