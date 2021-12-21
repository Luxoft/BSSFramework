using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.Service
{
    public interface IServiceBase<out TServiceEnvironment, out TBLLContext>// : IDBSessionEvaluator<TBLLContext>
        where TServiceEnvironment : class, IServiceEnvironment<TBLLContext>
    {
        TServiceEnvironment ServiceEnvironment { get; }
    }
}