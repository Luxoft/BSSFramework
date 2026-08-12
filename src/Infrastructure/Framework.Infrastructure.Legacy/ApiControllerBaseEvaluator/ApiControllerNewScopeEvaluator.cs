using Framework.Database;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Infrastructure.Services;

namespace Framework.Infrastructure.ApiControllerBaseEvaluator;

public class ApiControllerNewScopeEvaluator<TBLLContext, TMappingService>(IContextEvaluator<TBLLContext, TMappingService> contextEvaluator)
    : IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult) => contextEvaluator.Evaluate(sessionMode, getResult);
}
