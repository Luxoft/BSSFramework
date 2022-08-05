using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Configuration.BLL;
using Framework.Configuration.BLL.Notification;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Notification;
using Framework.QueryableSource;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterGenericServices(this IServiceCollection services)
        {
            services.AddSingleton<IExceptionExpander, ExceptionExpander>();

            services.AddScoped(sp => sp.GetRequiredService<IDBSession>().GetObjectStateService());

            services.AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();

            services.AddScoped<IStandardSubscriptionService, LocalDBSubscriptionService>();

            return services;
        }

        public static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
        {
            return services.AddScopedFrom<IAuthorizationSystem<Guid>, IAuthorizationBLLContext>();
        }

        public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
        {
            return services

                   .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetDALFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>())

                   .AddScoped<IOperationEventSenderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>, OperationEventSenderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>()

                   .AddScoped<BLLSourceEventListenerContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>()

                   .AddSingleton<AuthorizationValidatorCompileCache>()

                   .AddScoped<IAuthorizationValidator>(sp =>
                        new AuthorizationValidator(sp.GetRequiredService<IAuthorizationBLLContext>(), sp.GetRequiredService<AuthorizationValidatorCompileCache>()))

                   .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
                   .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
                   .AddScoped<IRunAsManager, AuthorizationRunAsManger>()
                   .AddScoped<IRuntimePermissionOptimizationService, RuntimePermissionOptimizationService>()
                   .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()
                   .AddScopedFromLazyInterfaceImplement<IAuthorizationBLLContext, AuthorizationBLLContext>()
                   .AddScopedFrom<Framework.DomainDriven.BLL.Configuration.IConfigurationBLLContext, IConfigurationBLLContext>()

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

                   .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetDALFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>())

                   .AddScoped<IOperationEventSenderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>, OperationEventSenderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>()

                   .AddScoped<BLLSourceEventListenerContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>()

                   .AddSingleton<ConfigurationValidatorCompileCache>()

                   .AddScoped<IConfigurationValidator>(sp =>
                        new ConfigurationValidator(sp.GetRequiredService<IConfigurationBLLContext>(), sp.GetRequiredService<ConfigurationValidatorCompileCache>()))


                   .AddSingleton(new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IConfigurationSecurityService, ConfigurationSecurityService>()
                   .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

                   .AddScopedFrom<ICurrentRevisionService, IDBSession>()

                   .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
                   .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()


                   .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
                   .AddScopedFromLazyInterfaceImplement<IConfigurationBLLContext, ConfigurationBLLContext>()

                   .AddScopedFrom<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>, IConfigurationBLLContext>()
                   .AddScopedFrom<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>, IConfigurationSecurityService>()
                   .AddScopedFrom<IConfigurationSecurityPathContainer, IConfigurationSecurityService>()
                   .AddScoped<IQueryableSource<Framework.Configuration.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectBase, Guid>>()
                   .AddScoped<ISecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
                   .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()

                   .Self(ConfigurationSecurityServiceBase.Register)
                   .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection AddScopedFromLazyInterfaceImplement<TInterface, TImplementation>(this IServiceCollection services)
            where TImplementation: class, TInterface
            where TInterface : class
        {
            return services.AddScoped<TImplementation>()
                           .AddScoped(sp => LazyInterfaceImplementHelper.CreateProxy<TInterface>(() => sp.GetRequiredService<TImplementation>()));
        }

        public static IServiceCollection AddScopedFromLazy<TInterface, TImplementation>(this IServiceCollection services)
                where TImplementation : class, TInterface
                where TInterface : class
        {
            return services.AddScoped<TImplementation>()
                           .AddScoped(sp => new Lazy<TInterface>(sp.GetRequiredService<TImplementation>))
                           .AddScoped(sp => sp.GetRequiredService<Lazy<TInterface>>().Value);
        }

        public static IServiceCollection AddScopedFrom<TSource, TImplementation>(this IServiceCollection services)
                where TImplementation : class, TSource
                where TSource : class
        {
            return services.AddScoped<TSource>(sp => sp.GetRequiredService<TImplementation>());
        }


        public static IServiceCollection AddSingletonFrom<TSource, TImplementation>(this IServiceCollection services)
                where TImplementation : class, TSource
                where TSource : class
        {
            return services.AddSingleton<TSource>(sp => sp.GetRequiredService<TImplementation>());
        }
    }
}
