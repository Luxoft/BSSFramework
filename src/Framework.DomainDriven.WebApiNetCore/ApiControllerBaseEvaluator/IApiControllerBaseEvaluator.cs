using System;

using Framework.DomainDriven.BLL;

public interface IApiControllerBaseEvaluator<out TEvaluatedData>
{
    TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult);
}
