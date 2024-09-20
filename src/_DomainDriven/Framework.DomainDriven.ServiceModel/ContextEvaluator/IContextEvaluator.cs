using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.ServiceModel;

public interface IContextEvaluator<TBLLContext, TMappingService>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string? customPrincipalName, Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult);
}
