using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;
using SecuritySystem.Credential;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public interface IRootServiceProviderContainer<out TBLLContext> : IRootServiceProviderContainer, IServiceEvaluator<TBLLContext>
{
    async Task<TResult> IServiceEvaluator<TBLLContext>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<TBLLContext, Task<TResult>> getResult)
    {
        return await this.RootServiceProvider.GetRequiredService<IServiceEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customUserCredential, getResult);
    }
}

public interface IRootServiceProviderContainer<TBLLContext, TMappingService> : IRootServiceProviderContainer<TBLLContext>, IContextEvaluator<TBLLContext, TMappingService>
{
    async Task<TResult> IContextEvaluator<TBLLContext, TMappingService>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            UserCredential? customUserCredential,
            Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult)
    {
        return await this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TMappingService>>().EvaluateAsync(sessionMode, customUserCredential, getResult);
    }
}
