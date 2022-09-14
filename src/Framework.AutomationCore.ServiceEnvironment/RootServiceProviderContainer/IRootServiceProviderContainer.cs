using System;
using System.Threading.Tasks;

using Framework.DomainDriven;
using Framework.DomainDriven.ServiceModel;
using Framework.DomainDriven.ServiceModel.Service;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public interface IRootServiceProviderContainer
{
    IServiceProvider RootServiceProvider { get; }
}

public interface IRootServiceProviderContainer<out TBLLContext> : IRootServiceProviderContainer, IContextEvaluator<TBLLContext>
{
    Task<TResult> IContextEvaluator<TBLLContext>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            string customPrincipalName,
            [NotNull] Func<TBLLContext, IDBSession, Task<TResult>> getResult)
    {
        return this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }
}

public interface IRootServiceProviderContainer<TBLLContext, TDTOMappingService> : IRootServiceProviderContainer<TBLLContext>, IContextEvaluator<TBLLContext, TDTOMappingService>
{
    Task<TResult> IContextEvaluator<TBLLContext, TDTOMappingService>.EvaluateAsync<TResult>(
            DBSessionMode sessionMode,
            string customPrincipalName,
            Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult)
    {
        return this.RootServiceProvider.GetRequiredService<IContextEvaluator<TBLLContext, TDTOMappingService>>().EvaluateAsync(sessionMode, customPrincipalName, getResult);
    }
}
