using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

public class ApiControllerNewScopeEvaluator<TBLLContext, TDTOMappingService> : IApiControllerBaseEvaluator<EvaluatedData<TBLLContext, TDTOMappingService>>
        where TBLLContext : class
        where TDTOMappingService : class
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
