using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore;

/// <summary>
/// Class ApiControllerBase.
/// </summary>
/// <typeparam name="TServiceEnvironment">The type of the t service environment.</typeparam>
/// <typeparam name="TBLLContext">The type of the TBLL context.</typeparam>
/// <typeparam name="TEvaluatedData">The type of the t evaluated data.</typeparam>
/// <seealso cref="ApiControllerBase{TBLLContext}" />
public abstract class ApiControllerBase<TBLLContext, TEvaluatedData> : ApiControllerBase<TBLLContext>
        where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
        where TEvaluatedData : EvaluatedData<TBLLContext>
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
    public void Evaluate(DBSessionMode sessionMode, Action<TEvaluatedData> action)
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
    public TResult Evaluate<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult)
    {
        return this.HttpContext.RequestServices.GetRequiredService<IApiControllerBaseEvaluator<TEvaluatedData>>().Evaluate(sessionMode, getResult);
    }

    /// <summary>
    /// Open DB Session and run action for only Read operations
    /// </summary>
    [NonAction]
    public void EvaluateRead(Action<TEvaluatedData> action)
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
    public TResult EvaluateRead<TResult>(Func<TEvaluatedData, TResult> getResult)
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
    public void EvaluateWrite(Action<TEvaluatedData> action)
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
    public TResult EvaluateWrite<TResult>(Func<TEvaluatedData, TResult> getResult)
    {
        if (getResult == null)
        {
            throw new ArgumentNullException(nameof(getResult));
        }

        return this.Evaluate(DBSessionMode.Write, getResult);
    }
}
