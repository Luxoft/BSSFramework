using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Configuration.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public abstract partial class ServiceEnvironmentBase : IServiceEnvironment<IConfigurationBLLContext>
    {
        IContextEvaluator<IConfigurationBLLContext> IServiceEnvironment<IConfigurationBLLContext>.GetContextEvaluator(IServiceProvider currentScopedServiceProvider)
        {
            return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<IConfigurationBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<IConfigurationBLLContext>(this, currentScopedServiceProvider);
        }
    }
}
