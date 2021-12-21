using System;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract partial class ServiceEnvironmentBase : IServiceEnvironment<IAuthorizationBLLContext>
    {
        IContextEvaluator<IAuthorizationBLLContext> IServiceEnvironment<IAuthorizationBLLContext>.GetContextEvaluator(IServiceProvider currentScopedServiceProvider)
        {
            return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<IAuthorizationBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<IAuthorizationBLLContext>(this, currentScopedServiceProvider);
        }
    }
}
