using Framework.Database;
using Framework.Infrastructure.Service;

namespace Framework.Infrastructure.ApiControllerBaseEvaluator;

public interface IApiControllerBaseEvaluator<TBLLContext, TMappingService>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult);
}
