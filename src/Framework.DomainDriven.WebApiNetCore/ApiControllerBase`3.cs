using System;
using System.Security.Cryptography;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <summary>
    /// Class ApiControllerBase.
    /// </summary>
    /// <typeparam name="TServiceEnvironment">The type of the t service environment.</typeparam>
    /// <typeparam name="TBLLContext">The type of the TBLL context.</typeparam>
    /// <typeparam name="TEvaluatedData">The type of the t evaluated data.</typeparam>
    /// <seealso cref="ApiControllerBase{TServiceEnvironment,TBLLContext}" />
    public abstract class ApiControllerBase<TServiceEnvironment, TBLLContext, TEvaluatedData> : ApiControllerBase<TServiceEnvironment, TBLLContext>
            where TServiceEnvironment : class, IServiceEnvironment<TBLLContext>
            where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>
            where TEvaluatedData : EvaluatedData<TBLLContext>
    {
        private IServiceProvider serviceProvider;

        protected ApiControllerBase(TServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
                : base(environment, exceptionProcessor)
        {
        }

        public override IServiceProvider ServiceProvider
        {
            get { return this.serviceProvider ?? this.HttpContext?.RequestServices; }
            set { this.serviceProvider = value; }
        }

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
            if (sessionMode == DBSessionMode.Read)
            {
                this.ServiceProvider.GetRequiredService<IDBSession>().AsReadOnly();
            }

            return getResult(this.ServiceProvider.GetRequiredService<TEvaluatedData>());
        }

        [NonAction]
        public TResult EvaluateWithNewScope<TResult>(DBSessionMode sessionMode, Func<TEvaluatedData, TResult> getResult)
        {
            if (sessionMode == DBSessionMode.Read)
            {
                this.ServiceProvider.GetRequiredService<IDBSession>().AsReadOnly();
            }

            return getResult(this.ServiceProvider.GetRequiredService<TEvaluatedData>());
        }

        /// <summary>
        /// Open DB Session and run action for only Read operations
        /// </summary>
        [NonAction]
        public void EvaluateRead([NotNull] Action<TEvaluatedData> action)
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
        public TResult EvaluateRead<TResult>([NotNull] Func<TEvaluatedData, TResult> getResult)
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
        public void EvaluateWrite([NotNull] Action<TEvaluatedData> action)
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
        public TResult EvaluateWrite<TResult>([NotNull] Func<TEvaluatedData, TResult> getResult)
        {
            if (getResult == null)
            {
                throw new ArgumentNullException(nameof(getResult));
            }

            return this.Evaluate(DBSessionMode.Write, getResult);
        }

        /// <summary>
        /// Get Data for Evaluate
        /// </summary>
        protected abstract TEvaluatedData GetEvaluatedData(IDBSession session, TBLLContext context);

        private Exception GetFacadeException(Exception exception)
        {
            try
            {
                return this.ExceptionProcessor.Process(exception);
            }
            catch (Exception)
            {
                return new InternalServerException();
            }
        }
    }
}
