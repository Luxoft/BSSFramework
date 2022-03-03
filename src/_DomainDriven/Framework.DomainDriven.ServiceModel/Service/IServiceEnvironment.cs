using System;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public interface IServiceEnvironment : IDBSessionFactoryContainer
    {

        bool IsDebugMode { get; }
    }

    public interface IServiceEnvironment<out TBLLContext> : IServiceEnvironment
    {
        IContextEvaluator<TBLLContext> GetContextEvaluator(IServiceProvider currentScopedServiceProvider = null);

        IBLLContextContainer<TBLLContext> GetBLLContextContainer(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName = null);
    }
}
