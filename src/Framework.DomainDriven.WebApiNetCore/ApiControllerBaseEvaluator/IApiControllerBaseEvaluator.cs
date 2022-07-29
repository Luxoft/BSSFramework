using System;

using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.WebApiNetCore;

public interface IApiControllerBaseEvaluator<out TEvaluatedData>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult);
}
