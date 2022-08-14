using System;
using System.Threading.Tasks;

namespace Framework.DomainDriven;

public interface IDBSessionEvaluator
{
    Task<TResult> EvaluateAsync<TResult>(DBSessionMode sessionMode, Func<IServiceProvider, IDBSession, Task<TResult>> getResult);
}
