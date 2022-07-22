using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <summary>
    ///     Class ApiControllerBase.
    /// </summary>
    /// <typeparam name="TBLLContext">The type of the TBLL context.</typeparam>
    public abstract class ApiControllerBase<TBLLContext> : ControllerBase, IApiControllerBase
        where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>,
        IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>
    {
        public abstract IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///     Open DB Session and run Action with pass BLLContext to him
        /// </summary>
        [NonAction]
        public abstract void EvaluateC(DBSessionMode sessionMode, Action<TBLLContext> action);

        /// <summary>
        ///     Open DB Session and run Func with pass BLLContext to him
        /// </summary>
        [NonAction]
        public abstract TResult EvaluateC<TResult>(DBSessionMode sessionMode, Func<TBLLContext, TResult> getResult);
    }
}
