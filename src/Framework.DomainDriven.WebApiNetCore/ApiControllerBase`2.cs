using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using Microsoft.AspNetCore.Mvc;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <summary>
    ///     Class ApiControllerBase.
    /// </summary>
    /// <typeparam name="TServiceEnvironment">The type of the t service environment.</typeparam>
    /// <typeparam name="TBLLContext">The type of the TBLL context.</typeparam>
    public abstract class ApiControllerBase<TServiceEnvironment, TBLLContext> : ControllerBase, IApiControllerBase
        where TServiceEnvironment : class, IServiceEnvironment
        where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>,
        IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>
    {
        protected ApiControllerBase(
            TServiceEnvironment serviceEnvironment,
            IExceptionProcessor exceptionProcessor)
        {
            this.ServiceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
            this.ExceptionProcessor = exceptionProcessor ?? throw new ArgumentNullException(nameof(exceptionProcessor));
        }


        public abstract IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        ///     Default Exception processor
        /// </summary>
        protected IExceptionProcessor ExceptionProcessor { get; }

        /// <summary>
        ///     Current ServiceEnvironment
        /// </summary>
        public TServiceEnvironment ServiceEnvironment { get; }

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
