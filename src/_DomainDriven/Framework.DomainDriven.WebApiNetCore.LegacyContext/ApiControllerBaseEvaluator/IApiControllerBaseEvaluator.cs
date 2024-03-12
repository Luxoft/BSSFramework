using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult);
}
