using Framework.Core;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.ServiceModel;

public class ContextEvaluator<TBLLContext, TMappingService> : IContextEvaluator<TBLLContext, TMappingService>
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
        Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult)
    {
        return await this.baseContextEvaluator.EvaluateAsync(
            sessionMode,
            customPrincipalName,
            context => getResult(
                new EvaluatedData<TBLLContext, TMappingService>(
                    context,
                    context.ServiceProvider.GetRequiredService<TMappingService>())));
    }
}
