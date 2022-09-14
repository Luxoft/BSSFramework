using System;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IApiControllerBaseEvaluator<out TEvaluatedData>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult);
}
