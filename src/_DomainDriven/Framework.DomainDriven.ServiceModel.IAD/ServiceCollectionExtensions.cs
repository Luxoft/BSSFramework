using System;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Persistent;
using Framework.QueryableSource;
using Framework.SecuritySystem;
using Framework.Workflow.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterEvaluateScopeManager<TBLLContext>(this IServiceCollection services)
        {
            return services
                   .AddScoped<IEvaluateScopeManager<TBLLContext>, EvaluateScopeManager<TBLLContext>>()
                   .AddScoped<IEvaluateScopeManager>(sp => sp.GetRequiredService<IEvaluateScopeManager<TBLLContext>>());
        }

        public static IServiceCollection RegisterAuthorizationSystem(this IServiceCollection services)
        {
            return services.AddScopedTransientByContainerBase<IAuthorizationSystem<Guid>>(c => c.Authorization);
        }

        public static IServiceCollection RegisterAuthorizationBLL(this IServiceCollection services)
        {
            return services
                .AddScopedTransientByContainerBase(c => c.Authorization)
                .AddScopedTransientByContainerBase<ISecurityOperationResolver<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.AuthorizationSecurityOperationCode>>(c => c.Authorization)
                .AddScopedTransientByContainerBase<IDisabledSecurityProviderContainer<Framework.Authorization.Domain.PersistentDomainObjectBase>>(c => c.Authorization.SecurityService)
                .AddScopedTransientByContainerBase<IAuthorizationSecurityPathContainer>(c => c.Authorization.SecurityService)
                .AddScopedTransientByContainerBase(c => c.Authorization.GetQueryableSource())
                .AddScopedTransientByContainerBase(c => c.Authorization.SecurityExpressionBuilderFactory)

                .AddScoped<IAccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Authorization.Domain.PersistentDomainObjectBase, Guid>>()
                .Self(AuthorizationSecurityServiceBase.Register)
                .Self(AuthorizationBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection RegisterConfigurationBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedTransientByContainerBase(c => c.Configuration)
                   .AddScopedTransientByContainerBase<ISecurityOperationResolver<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.ConfigurationSecurityOperationCode>>(c => c.Configuration)
                   .AddScopedTransientByContainerBase<IDisabledSecurityProviderContainer<Framework.Configuration.Domain.PersistentDomainObjectBase>>(c => c.Configuration.SecurityService)
                   .AddScopedTransientByContainerBase<IConfigurationSecurityPathContainer>(c => c.Configuration.SecurityService)
                   .AddScopedTransientByContainerBase(c => c.Configuration.GetQueryableSource())
                   .AddScopedTransientByContainerBase(c => c.Configuration.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Configuration.Domain.PersistentDomainObjectBase, Guid>>()
                   .Self(ConfigurationSecurityServiceBase.Register)
                   .Self(ConfigurationBLLFactoryContainer.RegisterBLLFactory);
        }


        public static IServiceCollection RegisterWorkflowBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedTransientByContainerBase(c => c.Workflow)
                   .AddScopedTransientByContainerBase<ISecurityOperationResolver<Framework.Workflow.Domain.PersistentDomainObjectBase, Framework.Workflow.WorkflowSecurityOperationCode>>(c => c.Workflow)
                   .AddScopedTransientByContainerBase<IDisabledSecurityProviderContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>>(c => c.Workflow.SecurityService)
                   .AddScopedTransientByContainerBase<IWorkflowSecurityPathContainer>(c => c.Workflow.SecurityService)
                   .AddScopedTransientByContainerBase(c => c.Workflow.GetQueryableSource())
                   .AddScopedTransientByContainerBase(c => c.Workflow.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>>()
                   .Self(WorkflowSecurityServiceBase.Register)
                   .Self(WorkflowBLLFactoryContainer.RegisterBLLFactory);
        }


        public static IServiceCollection AddScopedTransientByContainerBase<T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer, T> func)
            where T : class
        {
            return services.AddScopedTransientFactory(sp => sp.GetRequiredService<IEvaluateScopeManager>()
                                                              .Pipe(manager => FuncHelper.Create(() => func(manager.CurrentBLLContextContainer))));
        }

        public static IServiceCollection AddScopedTransientByContainer<TBLLContext, T>(this IServiceCollection services, Func<IServiceEnvironmentBLLContextContainer<TBLLContext>, T> func)
            where T : class
        {
            return services.AddScopedTransientFactory(sp => sp.GetRequiredService<IEvaluateScopeManager<TBLLContext>>()
                                                              .Pipe(manager => FuncHelper.Create(() => func(manager.CurrentBLLContextContainer))));
        }

        public static IServiceCollection AddScopedTransient<T>(this IServiceCollection services, Func<IServiceProvider, T> func)
            where T : class
        {
            return services.AddScopedTransientFactory(sp => FuncHelper.Create(() => func(sp)));
        }

        public static IServiceCollection AddScopedTransientFactory<T>(this IServiceCollection services, Func<IServiceProvider, Func<T>> getFunc)
            where T : class
        {
            return services.AddScoped(sp => getFunc(sp).Pipe(LazyInterfaceImplementHelper.CreateCallProxy));
        }
    }
}
