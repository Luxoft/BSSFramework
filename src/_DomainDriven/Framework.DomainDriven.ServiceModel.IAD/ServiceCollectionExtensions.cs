using System;

using Framework.Authorization.BLL;
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
        public static IServiceCollection RegisterGenericBLLServices(this IServiceCollection services)
        {
            return services.AddScoped(sp => sp.GetRequiredService<IDBSession>().GetObjectStateService())
                           .AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();
        }

        public static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationSystem<Guid>>(sp => sp.GetRequiredService<IAuthorizationBLLContext>());
        }

        public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
        {
            return services

                   .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetDALFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>())

                   .AddScoped<BLLOperationEventListenerContainer<Framework.Authorization.Domain.DomainObjectBase>>()
                   .AddScoped<BLLSourceEventListenerContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>()

                   .AddSingleton<AuthorizationValidatorCompileCache>()

                   .AddScoped<IAuthorizationValidator>(sp =>
                        new AuthorizationValidator(sp.GetRequiredService<IAuthorizationBLLContext>(), sp.GetRequiredService<AuthorizationValidatorCompileCache>()))

                   .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
                   .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
                   .AddScoped<IRunAsManager, AuthorizationRunAsManger>()
                   .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()
                   .AddLazyScoped<IAuthorizationBLLContext, AuthorizationBLLContext>()

                   .AddScoped<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.AuthorizationSecurityOperationCode>>(sp => sp.GetRequiredService<IAuthorizationBLLContext>())
                   .AddScoped<IDisabledSecurityProviderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>(sp => sp.GetRequiredService<IAuthorizationSecurityService>())
                   .AddScoped<IAuthorizationSecurityPathContainer>(sp => sp.GetRequiredService<IAuthorizationSecurityService>())
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

                   .AddScoped<BLLOperationEventListenerContainer<Framework.Configuration.Domain.DomainObjectBase>>()
                   .AddScoped<BLLSourceEventListenerContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>()

                   .AddSingleton<ConfigurationValidatorCompileCache>()

                   .AddScoped<IConfigurationValidator>(sp =>
                        new ConfigurationValidator(sp.GetRequiredService<IConfigurationBLLContext>(), sp.GetRequiredService<ConfigurationValidatorCompileCache>()))


                   .AddSingleton(new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IConfigurationSecurityService, ConfigurationSecurityService>()
                   .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

                   .AddScoped<ICurrentRevisionService>(sp => sp.GetRequiredService<IDBSession>())

                   .AddScoped<IMessageSender<Framework.Notification.MessageTemplateNotification>, TemplateMessageSender>()
                   .AddScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, LocalDBNotificationEventDTOMessageSender>()
                   .AddScoped<IExceptionService, ExceptionService>()


                   .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
                   .AddLazyScoped<IConfigurationBLLContext, ConfigurationBLLContext>()

                   .AddScoped<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>>(sp => sp.GetRequiredService<IConfigurationBLLContext>())
                   .AddScoped<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>(sp => sp.GetRequiredService<IConfigurationSecurityService>())
                   .AddScoped<IConfigurationSecurityPathContainer>(sp => sp.GetRequiredService<IConfigurationSecurityService>())
                   .AddScoped<IQueryableSource<Framework.Configuration.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectBase, Guid>>()
                   .AddScoped<ISecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
                   .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()

                   .Self(ConfigurationSecurityServiceBase.Register)
                   .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection AddLazyScoped<TInterface, TImplementation>(this IServiceCollection services)
            where TImplementation: TInterface
            where TInterface : class
        {
            return services.AddScoped(sp => LazyInterfaceImplementHelper.CreateProxy<TInterface>(() => ActivatorUtilities.CreateInstance<TImplementation>(sp)));
        }
    }
}
