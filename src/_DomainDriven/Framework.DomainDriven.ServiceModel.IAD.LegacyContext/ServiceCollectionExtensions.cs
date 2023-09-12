using Framework.Authorization.BLL;
using Framework.Authorization.Notification;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DependencyInjection;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Tracking;
using Framework.DomainDriven.NHibernate;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyGenericServices(this IServiceCollection services)
    {
        services.AddScoped(typeof(IOperationEventSenderContainer<>), typeof(OperationEventSenderContainer<>));

        services.AddScoped(typeof(IDAL<,>), typeof(NHibDal<,>));

        services.AddSingleton<IExceptionExpander, ExceptionExpander>();

        services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

        services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

        services.RegisterAuthorizationSystem();

        services.AddSingleton(AvailableValuesHelper.AvailableValues.ToValidation());

        services.AddSingleton<IDateTimeService>(DateTimeService.Default);

        services.AddSingleton<IPersistentInfoService, PersistentInfoService>();

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();

        return services;
    }

    public static IServiceCollection RegisterLegacyHierarchicalObjectExpander(this IServiceCollection services)
    {
        return services.ReplaceSingleton<IHierarchicalRealTypeResolver, ProjectionHierarchicalRealTypeResolver>();
    }

    private static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
    {
        return services.AddScopedFrom<IAuthorizationSystem<Guid>, IAuthorizationBLLContext>();
    }

    public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<AuthorizationValidationMap>()
               .AddSingleton<AuthorizationValidatorCompileCache>()
               .AddScoped<IAuthorizationValidator, AuthorizationValidator>()

               .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
               .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
               .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
               .AddScoped<IRunAsManager, AuthorizationRunAsManger>()
               .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()
               //.AddScoped<INotificationPrincipalExtractor, LegacyNotificationPrincipalExtractor>()
               .AddScoped<INotificationBasePermissionFilterSource, LegacyNotificationPrincipalExtractor>()
               .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()

               .AddScopedFromLazyInterfaceImplement<IAuthorizationBLLContext, AuthorizationBLLContext>()

               .AddScoped<ITrackingService<Framework.Authorization.Domain.PersistentDomainObjectBase>, TrackingService<Framework.Authorization.Domain.PersistentDomainObjectBase>>()

               .AddSingleton<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase>, AuthorizationSecurityOperationResolver>()
               .AddScopedFrom<IAuthorizationSecurityPathContainer, IAuthorizationSecurityService>()
               .AddScoped<IQueryableSource<Framework.Authorization.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.DomainObjectBase, Guid>>()
               .AddScoped<ISecurityExpressionBuilderFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()


               .Self(AuthorizationSecurityServiceBase.Register)
               .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
    {
        return services

               .AddSingleton<ConfigurationValidationMap>()
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

               .AddScoped<ITrackingService<Framework.Configuration.Domain.PersistentDomainObjectBase>, TrackingService<Framework.Configuration.Domain.PersistentDomainObjectBase>>()

               .AddScopedFrom<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext, IConfigurationBLLContext>()

               .AddScopedFrom<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase>, ConfigurationSecurityOperationResolver>()

               .AddScopedFrom<IConfigurationSecurityPathContainer, IConfigurationSecurityService>()
               .AddScoped<IQueryableSource<Framework.Configuration.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectBase, Guid>>()
               .AddScoped<ISecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()

               .Self(ConfigurationSecurityServiceBase.Register)
               .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
    }
}
