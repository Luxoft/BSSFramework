using System;

using Framework.Authorization.BLL;
using Framework.Configuration.BLL;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Workflow.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public interface IServiceEnvironmentBLLContextContainer :
        IAuthorizationBLLContextContainer<IAuthorizationBLLContext>,
        BLL.Configuration.IConfigurationBLLContextContainer<IConfigurationBLLContext>,
        IWorkflowBLLContextContainer
    {
        IDBSession Session { get; }

        IServiceProvider ScopedServiceProvider { get; }
    }

    public interface IServiceEnvironmentBLLContextContainer<out TBLLContext> : IServiceEnvironmentBLLContextContainer
    {
        TBLLContext MainContext { get; }
    }
}
