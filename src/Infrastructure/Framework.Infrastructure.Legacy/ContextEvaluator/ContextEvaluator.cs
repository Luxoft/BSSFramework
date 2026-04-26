using Framework.Application;
using Framework.Core;
using Framework.Database;
using Framework.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;

namespace Framework.Infrastructure.ContextEvaluator;

public class ContextEvaluator<TBLLContext, TMappingService>(IServiceEvaluator<TBLLContext> baseContextEvaluator)
    : IContextEvaluator<TBLLContext, TMappingService>
    where TBLLContext : IServiceProviderContainer where TMappingService : notnull
{
    public async Task<TResult> EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult) =>
        await baseContextEvaluator.EvaluateAsync(
            sessionMode,
            customUserCredential,
            context => getResult(
                new EvaluatedData<TBLLContext, TMappingService>(
                    context,
                    context.ServiceProvider.GetRequiredService<TMappingService>())));
}
