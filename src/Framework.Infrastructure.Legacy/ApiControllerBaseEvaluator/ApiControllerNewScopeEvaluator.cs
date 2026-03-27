using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerNewScopeEvaluator<TBLLContext, TMappingService>(IContextEvaluator<TBLLContext, TMappingService> contextEvaluator)
    : IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        return contextEvaluator.Evaluate(sessionMode, getResult);
    }
}
