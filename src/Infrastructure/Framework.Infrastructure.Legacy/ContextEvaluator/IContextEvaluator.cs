using Framework.Database;
using Framework.Infrastructure.Services;

using Anch.SecuritySystem;

namespace Framework.Infrastructure.ContextEvaluator;

public interface IContextEvaluator<TBLLContext, TMappingService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult);
}
