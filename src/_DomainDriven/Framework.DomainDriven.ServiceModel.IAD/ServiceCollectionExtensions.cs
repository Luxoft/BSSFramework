using System;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
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
            return services.AddScoped<AvailableValues>()
                           .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetObjectStateService())
                           .AddSingleton<IStandartExpressionBuilder, StandartExpressionBuilder>();
        }

        public static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
        {
            return services.AddScoped<IAuthorizationSystem<Guid>, IAuthorizationBLLContext>();
        }

        public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
        {
            return services

                   .AddScoped(sp => sp.GetRequiredService<IDBSession>().GetDALFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>())

                   .AddScoped<BLLOperationEventListenerContainer<Framework.Authorization.Domain.DomainObjectBase>>()
                   .AddScoped<BLLSourceEventListenerContainer<Framework.Authorization.Domain.DomainObjectBase>>()

                   .AddSingleton<AuthorizationValidatorCompileCache>()

                   .AddScoped<IAuthorizationValidator>(sp =>
                        new AuthorizationValidator(sp.GetRequiredService<IAuthorizationBLLContext>(), sp.GetRequiredService<AuthorizationValidatorCompileCache>()))

                   .AddSingleton(new AuthorizationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Authorization.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IAuthorizationSecurityService, AuthorizationSecurityService>()
                   .AddScoped<IAuthorizationBLLFactoryContainer, AuthorizationBLLFactoryContainer>()
                   .AddScoped<IRunAsManager, AuthorizationRunAsManger>()
                   .AddScoped<IAuthorizationBLLContextSettings, AuthorizationBLLContextSettings>()
                   .AddScoped<IAuthorizationBLLContext, AuthorizationBLLContext>()

                   .AddScoped<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.AuthorizationSecurityOperationCode>, IAuthorizationBLLContext>()
                   .AddScoped<IDisabledSecurityProviderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>, IAuthorizationSecurityService>()
                   .AddScoped<IAuthorizationSecurityPathContainer, IAuthorizationSecurityService>()
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
                   .AddScoped<BLLSourceEventListenerContainer<Framework.Configuration.Domain.DomainObjectBase>>()

                   .AddSingleton<ConfigurationValidatorCompileCache>()

                   .AddScoped<IConfigurationValidator>(sp =>
                        new ConfigurationValidator(sp.GetRequiredService<IConfigurationBLLContext>(), sp.GetRequiredService<ConfigurationValidatorCompileCache>()))


                   .AddSingleton(new ConfigurationMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Configuration.Domain.PersistentDomainObjectBase>.OData))
                   .AddScoped<IConfigurationSecurityService, ConfigurationSecurityService>()
                   .AddScoped<IConfigurationBLLFactoryContainer, ConfigurationBLLFactoryContainer>()

                   .AddScoped<ICurrentRevisionService, IDBSession>()

                   .AddScoped<IConfigurationBLLContextSettings, ConfigurationBLLContextSettings>()
                   .AddScoped<IConfigurationBLLContext, ConfigurationBLLContext>()

                   .AddScoped<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>, IConfigurationBLLContext>()
                   .AddScoped<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>, IConfigurationSecurityService>()
                   .AddScoped<IConfigurationSecurityPathContainer, IConfigurationSecurityService>()
                   .AddScoped<IQueryableSource<Framework.Configuration.Domain.PersistentDomainObjectBase>, BLLQueryableSource<IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectBase, Guid>>()
                   .AddScoped<ISecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>, Framework.SecuritySystem.Rules.Builders.MaterializedPermissions.SecurityExpressionBuilderFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
                   .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()

                   .Self(ConfigurationSecurityServiceBase.Register)
                   .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
        }
    }
}
