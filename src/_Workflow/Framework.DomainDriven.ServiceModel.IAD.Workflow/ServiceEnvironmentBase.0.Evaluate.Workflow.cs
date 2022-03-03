using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Workflow.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract partial class ServiceEnvironmentBase : IServiceEnvironment<IWorkflowBLLContext>
    {
        IContextEvaluator<IWorkflowBLLContext> IServiceEnvironment<IWorkflowBLLContext>.GetContextEvaluator(IServiceProvider currentScopedServiceProvider)
        {
            return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<IWorkflowBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<IWorkflowBLLContext>(this, currentScopedServiceProvider);
        }
    }
}
