using System;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Framework.DomainDriven;

public interface IContextEvaluator<out TBLLContext>
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, string customPrincipalName, [NotNull] Func<TBLLContext, IDBSession, Task<TResult>> getResult);
}
