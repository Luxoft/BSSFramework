using Framework.DomainDriven.ServiceModel.IAD;

namespace Framework.DomainDriven.ServiceModel
{
    public interface IEvaluateScopeManager
    {
        IServiceEnvironmentBLLContextContainer CurrentBLLContextContainer { get; }

        void BeginScope(object bllContextContainer);

        void EndScope(object bllContextContainer);
    }

    public interface IEvaluateScopeManager<out TMainBLLContext> : IEvaluateScopeManager
    {
        new IServiceEnvironmentBLLContextContainer<TMainBLLContext> CurrentBLLContextContainer { get; }
    }
}
