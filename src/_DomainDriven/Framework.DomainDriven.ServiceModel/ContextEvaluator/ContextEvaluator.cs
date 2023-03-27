using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel;

public class ContextEvaluator<TBLLContext, TDTOMappingService> : IContextEvaluator<TBLLContext, TDTOMappingService>
    where TBLLContext : IServiceProviderContainer
{
    private readonly IContextEvaluator<TBLLContext> baseContextEvaluator;

    public ContextEvaluator(IContextEvaluator<TBLLContext> baseContextEvaluator)
    {
        this.baseContextEvaluator = baseContextEvaluator;
    }

    public Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        string customPrincipalName,
        Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return this.baseContextEvaluator.EvaluateAsync(sessionMode, customPrincipalName, (ctx, session) => getResult(new EvaluatedData<TBLLContext, TDTOMappingService>(session, ctx, ctx.ServiceProvider.GetRequiredService<TDTOMappingService>())));
    }
}
