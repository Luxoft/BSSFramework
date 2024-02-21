using Framework.Core;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel;

public class ContextEvaluator<TBLLContext, TDTOMappingService> : IContextEvaluator<TBLLContext, TDTOMappingService>
    where TBLLContext : IServiceProviderContainer
{
    private readonly IServiceEvaluator<TBLLContext> baseContextEvaluator;

    public ContextEvaluator(IServiceEvaluator<TBLLContext> baseContextEvaluator)
    {
        this.baseContextEvaluator = baseContextEvaluator;
    }

    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        string customPrincipalName,
        Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return await this.baseContextEvaluator.EvaluateAsync(
            sessionMode,
            customPrincipalName,
            context => getResult(
                new EvaluatedData<TBLLContext, TDTOMappingService>(
                    context,
                    context.ServiceProvider.GetRequiredService<TDTOMappingService>())));
    }
}
