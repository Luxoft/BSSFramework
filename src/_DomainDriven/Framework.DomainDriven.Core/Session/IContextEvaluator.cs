using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IContextEvaluator<out TBLLContext>
    {
        Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, IDBSession, Task<TResult>> getResult);

        TResult Evaluate<TResult>(DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult)
        {
            try
            {
                return this.EvaluateAsync(sessionMode, customPrincipalName, (c, s) => Task.FromResult(getResult(c, s))).Result;
            }
            catch (AggregateException ex) when (ex.InnerExceptions.Count == 1)
            {
                throw ex.InnerExceptions[0];
            }
        }
    }
}
