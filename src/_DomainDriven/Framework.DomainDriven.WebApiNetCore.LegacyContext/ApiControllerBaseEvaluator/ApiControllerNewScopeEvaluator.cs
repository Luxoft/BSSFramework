using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.WebApiNetCore;

public class ApiControllerNewScopeEvaluator<TBLLContext, TMappingService> : IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    private readonly IContextEvaluator<TBLLContext, TMappingService> contextEvaluator;

    public ApiControllerNewScopeEvaluator(IContextEvaluator<TBLLContext, TMappingService> contextEvaluator)
    {
        this.contextEvaluator = contextEvaluator;
    }

    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        return this.contextEvaluator.Evaluate(sessionMode, getResult);
    }
}
