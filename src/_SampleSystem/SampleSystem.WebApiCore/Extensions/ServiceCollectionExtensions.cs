using System;

using Framework.Core;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;
using SampleSystem.BLL.Core.Jobs;
using SampleSystem.BLL.Jobs;
using SampleSystem.Domain;

namespace SampleSystem.WebApiCore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterLegacyBLLContext(this IServiceCollection services)
    {
        services.RegisterAuthorizationSystem();

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();
        services.RegisterMainBLL();

        return services;
    }



    public static IServiceCollection RegisterMainBLL(this IServiceCollection services)
    {
        return services

               .AddScopedByContainer(c => c.MainContext)
               .AddScopedByContainer<ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode>>(c => c.MainContext)
               .AddScopedByContainer<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>>(c => c.MainContext.SecurityService)
               .AddScopedByContainer<ISampleSystemSecurityPathContainer>(c => c.MainContext.SecurityService)
               .AddScopedByContainer(c => c.MainContext.GetQueryableSource())
               .AddScopedByContainer(c => c.MainContext.SecurityExpressionBuilderFactory)

               .AddScoped<IAccessDeniedExceptionService<PersistentDomainObjectBase>, AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>>()
               .Self(SampleSystemSecurityServiceBase.Register)
               .Self(SampleSystemBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection AddScopedByContainer<T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer<ISampleSystemBLLContext>, T> func)
            where T : class
    {
        return services.AddScoped(sp => sp.GetRequiredService<IServiceEnvironmentBLLContextContainer<ISampleSystemBLLContext>>().Pipe(func));
    }

    public static IServiceCollection RegisterDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnvironment(configuration);

        services.AddScoped<ISampleJob, SampleJob>();

        return services;
    }
}
