using Framework.Application.Session;
using Framework.BLL.ServiceModel.Service;

using SecuritySystem.Credential;

namespace Framework.BLL.ServiceModel.ContextEvaluator;

public interface IContextEvaluator<TBLLContext, TMappingService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult);
}
