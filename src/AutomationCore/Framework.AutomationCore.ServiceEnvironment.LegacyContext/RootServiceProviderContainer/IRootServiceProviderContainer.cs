using Framework.Application;
using Framework.Database;
using Framework.Infrastructure.ContextEvaluator;
using Framework.Infrastructure.Service;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Credential;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public interface IRootServiceProviderContainer<out TBLLContext> : IRootServiceProviderContainer, IServiceEvaluator<TBLLContext>
{
    async Task<TResult> IServiceEvaluator<TBLLContext>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<TBLLContext, Task<TResult>> getResult) =>
        await this.RootServiceProvider.GetRequiredService<IServiceEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customUserCredential, getResult);
}

public interface IRootServiceProviderContainer<TBLLContext, TMappingService> : IRootServiceProviderContainer<TBLLContext>, IContextEvaluator<TBLLContext, TMappingService>
{
    async Task<TResult> IContextEvaluator<TBLLContext, TMappingService>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult) =>
        await this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TMappingService>>().EvaluateAsync(sessionMode, customUserCredential, getResult);
}
