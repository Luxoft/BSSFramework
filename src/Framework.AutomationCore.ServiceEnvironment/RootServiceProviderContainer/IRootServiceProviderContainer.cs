using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }
}

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

public interface IRootServiceProviderContainer<TBLLContext, TDTOMappingService> : IRootServiceProviderContainer<TBLLContext>, IContextEvaluator<TBLLContext, TDTOMappingService>
{
    async Task<TResult> IContextEvaluator<TBLLContext, TDTOMappingService>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            string customPrincipalName,
            Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return await this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TDTOMappingService>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }
}
