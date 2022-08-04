using System;
using System.Threading.Tasks;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.ServiceModel;

public interface IContextEvaluator<TBLLContext, TDTOMappingService>
    where TBLLContext : class
    where TDTOMappingService : class
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<EvaluatedData<TBLLContext, TDTOMappingService>, Task<TResult>> getResult);

    TResult Evaluate<TResult>(DBSessionMode sessionMode, string customPrincipalName, Func<EvaluatedData<TBLLContext, TDTOMappingService>, TResult> getResult)
    {
        try
        {
            return this.EvaluateAsync(sessionMode, customPrincipalName, c => Task.FromResult(getResult(c))).Result;
        }
        catch (AggregateException ex) when (ex.InnerExceptions.Count == 1)
        {
            throw ex.InnerExceptions[0];
        }
    }
}
