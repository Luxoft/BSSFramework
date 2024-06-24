using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

/// <summary>
/// Class ApiControllerBase.
/// </summary>
/// <typeparam name="TBLLContext">The type of the TBLL context.</typeparam>
/// <typeparam name="EvaluatedData<TBLLContext, TMappingService>">The type of the t evaluated data.</typeparam>
/// <seealso cref="ApiControllerBase{TBLLContext}" />
public abstract class ApiControllerBase<TBLLContext, TMappingService> : ApiControllerBase<TBLLContext>
        where TBLLContext : class
{
    /// <inheritdoc />
    [NonAction]
    public sealed override void EvaluateC(DBSessionMode sessionMode, Action<TBLLContext> action)
    {
        this.Evaluate(sessionMode, data => action(data.Context));
    }

    /// <inheritdoc />
    [NonAction]
    public sealed override TResult EvaluateC<TResult>(DBSessionMode sessionMode, Func<TBLLContext, TResult> getResult)
    {
        return this.Evaluate(sessionMode, data => getResult(data.Context));
    }

    /// <summary>
    /// Open DB Session and run action
    /// </summary>
    [NonAction]
    public void Evaluate(DBSessionMode sessionMode, Action<EvaluatedData<TBLLContext, TMappingService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        this.Evaluate(sessionMode, data =>
                                   {
                                       action(data);
                                       return default(object);
                                   });
    }

    /// <summary>
    /// Open DB Session and run Func
    /// </summary>
    [NonAction]
    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        return this.HttpContext.RequestServices.GetRequiredService<IApiControllerBaseEvaluator<TBLLContext, TMappingService>>().Evaluate(sessionMode, getResult);
    }

    /// <summary>
    /// Open DB Session and run action for only Read operations
    /// </summary>
    [NonAction]
    public void EvaluateRead(Action<EvaluatedData<TBLLContext, TMappingService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        this.Evaluate(DBSessionMode.Read, action);
    }

    /// <summary>
    /// Open DB Session and run action for only Read operations
    /// </summary>
    [NonAction]
    public TResult EvaluateRead<TResult>(Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        if (getResult == null)
        {
            throw new ArgumentNullException(nameof(getResult));
        }

        return this.Evaluate(DBSessionMode.Read, getResult);
    }

    /// <summary>
    /// Open DB Session and run action for only Read/Write operations
    /// </summary>
    [NonAction]
    public void EvaluateWrite(Action<EvaluatedData<TBLLContext, TMappingService>> action)
    {
        if (action == null)
        {
            throw new ArgumentNullException(nameof(action));
        }

        this.Evaluate(DBSessionMode.Write, action);
    }

    /// <summary>
    /// Open DB Session and run action for only Read/Write operations
    /// </summary>
    [NonAction]
    public TResult EvaluateWrite<TResult>(Func<EvaluatedData<TBLLContext, TMappingService>, TResult> getResult)
    {
        if (getResult == null)
        {
            throw new ArgumentNullException(nameof(getResult));
        }

        return this.Evaluate(DBSessionMode.Write, getResult);
    }
}
