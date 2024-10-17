using Framework.DomainDriven.ServiceModel.Service;
using Framework.SecuritySystem.Credential;

namespace Framework.DomainDriven.ServiceModel;

public interface IContextEvaluator<TBLLContext, TMappingService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, UserCredential? customUserCredential, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult);
}
