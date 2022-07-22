using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <inheritdoc />
    public class ApiControllerExceptionService<TBLLContext> : IExceptionProcessor
        where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBLLContext> contextEvaluator;

        private readonly IDebugModeManager debugModeManager;

        public ApiControllerExceptionService(
                [NotNull] IContextEvaluator<TBLLContext> contextEvaluator,
                IDebugModeManager debugModeManager = null,
                bool expandDetailException = true)
        {
            this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
            this.debugModeManager = debugModeManager;

            this.ExpandDetailException = expandDetailException;
        }

        /// <summary>
        ///     Expand Detail Exception
        /// </summary>
        protected bool ExpandDetailException { get; }

        /// <inheritdoc />
        public Exception Process(Exception exception) =>
            this.contextEvaluator.Evaluate(DBSessionMode.Write, context => this.Process(exception, context));

        /// <summary>
        ///     Is Handled Exception
        /// </summary>
        protected virtual bool IsHandledException(Exception exception)
        {
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            var exceptionType = exception.GetType();

            var expectedExceptions = new[]
                                     {
                                         typeof(BusinessLogicException),
                                         typeof(IntergationException),
                                         typeof(SecurityException),
                                         typeof(ValidationException),
                                         typeof(DALException),
                                         typeof(StaleDomainObjectStateException)
                                     };

            return exceptionType.IsAssignableToAny(expectedExceptions);
        }

        /// <summary>
        ///     Get Internal Server Exception
        /// </summary>
        protected virtual Exception GetInternalServerException() =>
            new Exception(InternalServerException.DefaultMessage);

        private Exception Process(Exception exception, TBLLContext context)
        {
            var evaluateException = this.ToEvaluateException(exception, context);

            var expandedBaseException = evaluateException.ExpandedBaseException;

            var baseException = evaluateException.BaseException;

            this.Save(expandedBaseException, context);

            if (!this.debugModeManager.Maybe(v => v.IsDebugMode))
            {
                return this.GetFacadeException(expandedBaseException, context);
            }

            if (expandedBaseException == baseException)
            {
                return evaluateException;
            }

            return expandedBaseException;

        }

        private EvaluateException ToEvaluateException(Exception exception, TBLLContext context)
        {
            if (exception is EvaluateException evaluateException)
            {
                return evaluateException;
            }

            return new EvaluateException(exception, context.Configuration.ExceptionService.Process(exception));
        }

        private void Save([NotNull] Exception exception, TBLLContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            this.TrySaveExceptionMessage(exception, context);
        }

        private Exception GetFacadeException(Exception exception, TBLLContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            return this.IsHandledException(exception) || context.Configuration.DisplayInternalError
                       ? exception
                       : this.GetInternalServerException();
        }

        private void TrySaveExceptionMessage(Exception exception, TBLLContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            try
            {
                context.Configuration.ExceptionService.Save(exception);
            }
            catch
            {
            }
        }
    }
}
