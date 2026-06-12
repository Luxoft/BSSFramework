using Anch.SecuritySystem;

using Framework.Application;
using Framework.BLL;
using Framework.Database;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public interface IRootServiceProviderContainer<out TBLLContext> : IRootServiceProviderContainer,
                                                                  IServiceEvaluator<TBLLContext>,
                                                                  ISyncServiceEvaluator<TBLLContext>
{
    async Task<TResult> IServiceEvaluator<TBLLContext>.EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<TBLLContext, Task<TResult>> getResult,
        CancellationToken ct) =>
        await this.RootServiceProvider.GetRequiredService<IServiceEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customUserCredential, getResult, ct);

    TResult ISyncServiceEvaluator<TBLLContext>.Evaluate<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<TBLLContext, TResult> getResult) =>
        this.RootServiceProvider.GetRequiredService<ISyncServiceEvaluator<TBLLContext>>().Evaluate(sessionMode, customUserCredential, getResult);
}

public interface IRootServiceProviderContainer<TBLLContext, TMappingService> : IRootServiceProviderContainer<TBLLContext>,
                                                                               IContextEvaluator<TBLLContext, TMappingService>
{
    async Task<TResult> IContextEvaluator<TBLLContext, TMappingService>.EvaluateAsync<TResult>(
        DBSessionMode sessionMode,
        UserCredential? customUserCredential,
        Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult,
        CancellationToken ct) =>
        await this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TMappingService>>()
                  .EvaluateAsync(sessionMode, customUserCredential, getResult, ct);
}
