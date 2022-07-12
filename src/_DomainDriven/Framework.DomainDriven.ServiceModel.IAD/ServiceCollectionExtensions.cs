using System;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
        {
            return services.AddScopedByContainerBase<IAuthorizationSystem<Guid>>(c => c.Authorization);
        }

        public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
        {
            return services
                .AddScopedByContainerBase(c => c.Authorization)
                .AddScopedByContainerBase<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.AuthorizationSecurityOperationCode>>(c => c.Authorization)
                .AddScopedByContainerBase<IDisabledSecurityProviderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>(c => c.Authorization.SecurityService)
                .AddScopedByContainerBase<IAuthorizationSecurityPathContainer>(c => c.Authorization.SecurityService)
                .AddScopedByContainerBase(c => c.Authorization.GetQueryableSource())
                .AddScopedByContainerBase(c => c.Authorization.SecurityExpressionBuilderFactory)

                .AddScoped<IAccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()
                .Self(AuthorizationSecurityServiceBase.Register)
                .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedByContainerBase(c => c.Configuration)
                   .AddScopedByContainerBase<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>>(c => c.Configuration)
                   .AddScopedByContainerBase<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>(c => c.Configuration.SecurityService)
                   .AddScopedByContainerBase<IConfigurationSecurityPathContainer>(c => c.Configuration.SecurityService)
                   .AddScopedByContainerBase(c => c.Configuration.GetQueryableSource())
                   .AddScopedByContainerBase(c => c.Configuration.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
                   .Self(ConfigurationSecurityServiceBase.Register)
                   .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
        }


        public static IServiceCollection AddScopedByContainerBase<T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer, T> func)
            where T : class
        {
            return services.AddScoped(sp => sp.GetRequiredService<IServiceEnvironmentBLLContextContainer>().Pipe(func));
        }
    }
}
