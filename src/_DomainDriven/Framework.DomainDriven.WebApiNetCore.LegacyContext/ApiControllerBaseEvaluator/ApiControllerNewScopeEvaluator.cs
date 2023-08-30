using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerNewScopeEvaluator<TBLLContext, TDTOMappingService> : IApiControllerBaseEvaluator<EvaluatedData<TBLLContext, TDTOMappingService>>
{
    private readonly IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator;

    public ApiControllerNewScopeEvaluator(IContextEvaluator<TBLLContext, TDTOMappingService> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator;
    }

    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        return this.contextEvaluator.Evaluate(sessionMode, getResult);
    }
}
