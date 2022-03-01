using System;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.SecuritySystem;
using Framework.Workflow.BLL;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public static class WorkflowServiceCollectionExtensions
    {
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
    }
}
