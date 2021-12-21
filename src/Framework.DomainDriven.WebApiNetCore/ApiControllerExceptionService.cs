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
    public class ApiControllerExceptionService<TServiceEnvironment, TBLLContext> : IExceptionProcessor
        where TBLLContext : class, IConfigurationBLLContextContainer<IConfigurationBLLContext>
        where TServiceEnvironment : IServiceEnvironment<TBLLContext>
    {
        private readonly TServiceEnvironment serviceEnvironment;

        public ApiControllerExceptionService(
            [NotNull] TServiceEnvironment serviceEnvironment,
            bool expandDetailException = true)
        {
            if (serviceEnvironment == null)
            {
                throw new ArgumentNullException(nameof(serviceEnvironment));
            }

            this.serviceEnvironment = serviceEnvironment;

            this.ExpandDetailException = expandDetailException;
        }

        /// <summary>
        ///     Expand Detail Exception
        /// </summary>
        protected bool ExpandDetailException { get; }

        /// <inheritdoc />
        public Exception Process(Exception exception) =>
            this.serviceEnvironment.GetContextEvaluator().Evaluate(DBSessionMode.Write, context => this.Process(exception, context));

        /// <summary>
        ///     Safe Send To Mail Exception
        /// </summary>
        protected virtual void SafeSendToMailException(Exception baseException, TBLLContext context)
        {
            var tryMethod = new TryMethod<Exception, TBLLContext, Exception>(this.TrySendToMailException);

            Maybe.OfTryMethod(tryMethod)(baseException, context)
                 .ToReference()
                 .Maybe(z => this.TrySaveExceptionMessage(z, context));
        }

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

            if (!this.serviceEnvironment.IsDebugMode)
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

            this.SafeSendToMailException(exception, context);
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

        private bool TrySendToMailException(
            Exception baseException,
            TBLLContext context,
            out Exception faultSendException)
        {
            try
            {
                context.Configuration.ExceptionSender.Send(baseException, TransactionMessageMode.InternalTransaction);

                faultSendException = null;

                return true;
            }
            catch (Exception ex)
            {
                faultSendException = ex;

                return false;
            }
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
            catch (Exception ex)
            {
                this.SafeSendToMailException(ex, context);
            }
        }
    }
}
