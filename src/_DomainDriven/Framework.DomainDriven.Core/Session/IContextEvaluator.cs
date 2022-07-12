using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IContextEvaluator<out TBLLContext>
    {
        Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string principalName, [NotNull] Func<TBLLContext, IDBSession, Task<TResult>> getResult);
    }
}
