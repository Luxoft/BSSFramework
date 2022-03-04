using System;

using Framework.Core;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;
using Framework.Workflow.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace WorkflowSampleSystem.ServiceEnvironment
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection RegisterWorkflowBLL(this IServiceCollection services)
        {
            return services

                   .AddScopedTransientByWFContainer(c => c.Workflow)
                   .AddScopedTransientByWFContainer<ISecurityOperationResolver<Framework.Workflow.Domain.PersistentDomainObjectBase, Framework.Workflow.WorkflowSecurityOperationCode>>(c => c.Workflow)
                   .AddScopedTransientByWFContainer<IDisabledSecurityProviderContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>>(c => c.Workflow.SecurityService)
                   .AddScopedTransientByWFContainer<IWorkflowSecurityPathContainer>(c => c.Workflow.SecurityService)
                   .AddScopedTransientByWFContainer(c => c.Workflow.GetQueryableSource())
                   .AddScopedTransientByWFContainer(c => c.Workflow.SecurityExpressionBuilderFactory)

                   .AddScoped<IAccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase>, AccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>>()
                   .Self(WorkflowSecurityServiceBase.Register)
                   .Self(WorkflowBLLFactoryContainer.RegisterBLLFactory);
        }

        public static IServiceCollection AddScopedTransientByWFContainer<T>(this IServiceCollection services, Func<IWorkflowBLLContextContainer, T> func)
                where T : class
        {
            return services.AddScopedTransientByContainerBase(container => func((IWorkflowBLLContextContainer)container));
        }
    }
}
