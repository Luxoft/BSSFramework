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
        services.RegisterEvaluateScopeManager<ISampleSystemBLLContext>();
        services.RegisterAuthorizationSystem();

        services.RegisterAuthorizationBLL();
        services.RegisterConfigurationBLL();
        services.RegisterMainBLL();

        return services;
    }



    public static IServiceCollection RegisterMainBLL(this IServiceCollection services)
    {
        return services

               .AddScopedTransientByContainer(c => c.MainContext)
               .AddScopedTransientByContainer<ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode>>(c => c.MainContext)
               .AddScopedTransientByContainer<IDisabledSecurityProviderContainer<PersistentDomainObjectBase>>(c => c.MainContext.SecurityService)
               .AddScopedTransientByContainer<ISampleSystemSecurityPathContainer>(c => c.MainContext.SecurityService)
               .AddScopedTransientByContainer(c => c.MainContext.GetQueryableSource())
               .AddScopedTransientByContainer(c => c.MainContext.SecurityExpressionBuilderFactory)

               .AddScoped<IAccessDeniedExceptionService<PersistentDomainObjectBase>, AccessDeniedExceptionService<PersistentDomainObjectBase, Guid>>()
               .Self(SampleSystemSecurityServiceBase.Register)
               .Self(SampleSystemBLLFactoryContainer.RegisterBLLFactory);
    }

    public static IServiceCollection AddScopedTransientByContainer<T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer<ISampleSystemBLLContext>, T> func)
            where T : class
    {
        return services.AddScopedTransientFactory(sp => sp.GetRequiredService<IEvaluateScopeManager<ISampleSystemBLLContext>>()
                                                          .Pipe(manager => FuncHelper.Create(() => func(manager.CurrentBLLContextContainer))));
    }

    public static IServiceCollection RegisterDependencyInjections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEnvironment(configuration);

        services.AddScoped<ISampleJob, SampleJob>();

        return services;
    }
}
