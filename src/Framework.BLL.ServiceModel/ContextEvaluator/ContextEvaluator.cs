using Framework.Application;
using Framework.BLL.ServiceModel.Service;
using Framework.Core;
using Framework.Database;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Credential;

namespace Framework.BLL.ServiceModel.ContextEvaluator;

public class ContextEvaluator<TBLLContext, TMappingService>(IServiceEvaluator<TBLLContext> baseContextEvaluator)
    : IContextEvaluator<TBLLContext, TMappingService>
    where TBLLContext : IServiceProviderContainer where TMappingService : notnull
{
    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult)
    {
        return await baseContextEvaluator.EvaluateAsync(
                   sessionMode,
                   customUserCredential,
                   context => getResult(
                       new EvaluatedData<TBLLContext, TMappingService>(
                           context,
                           context.ServiceProvider.GetRequiredService<TMappingService>())));
    }
}
