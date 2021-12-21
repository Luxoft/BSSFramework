using System;

using JetBrains.Annotations;

namespace Framework.DomainDriven.BLL
{
    public interface IContextEvaluator<out TBLLContext>
    {

        TResult Evaluate<TResult>(DBSessionMode sessionMode, string principalName, [NotNull] Func<TBLLContext, IDBSession, TResult> getResult);
    }
}
