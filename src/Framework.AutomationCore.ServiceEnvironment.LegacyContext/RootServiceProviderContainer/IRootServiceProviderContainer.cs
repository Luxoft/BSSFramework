using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public interface IRootServiceProviderContainer<out TBLLContext> : IRootServiceProviderContainer, IServiceEvaluator<TBLLContext>
{
    async Task<TResult> IServiceEvaluator<TBLLContext>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            string customPrincipalName,
            Func<TBLLContext, Task<TResult>> getResult)
    {
        return await this.RootServiceProvider.GetRequiredService<IServiceEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }
}

public interface IRootServiceProviderContainer<TBLLContext, TMappingService> : IRootServiceProviderContainer<TBLLContext>, IContextEvaluator<TBLLContext, TMappingService>
{
    async Task<TResult> IContextEvaluator<TBLLContext, TMappingService>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            string customPrincipalName,
            Func<EvaluatedData<TBLLContext, TMappingService>, Task<TResult>> getResult)
    {
        return await this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TMappingService>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }
}
