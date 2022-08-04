using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.Exceptions;

using JetBrains.Annotations;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <inheritdoc />
    public class ApiControllerExceptionService : IRootExceptionService
    {
        private readonly IContextEvaluator<IConfigurationBLLContext> contextEvaluator;

        private readonly IApiControllerPostProcessExceptionService apiControllerPostProcessExceptionService;

        public ApiControllerExceptionService(
                [NotNull] IContextEvaluator<IConfigurationBLLContext> contextEvaluator,
                IApiControllerPostProcessExceptionService apiControllerPostProcessExceptionService,
                bool expandDetailException = true)
        {
            this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
            this.apiControllerPostProcessExceptionService = apiControllerPostProcessExceptionService ?? throw new ArgumentNullException(nameof(apiControllerPostProcessExceptionService));

            this.ExpandDetailException = expandDetailException;
        }

        /// <summary>
        ///     Expand Detail Exception
        /// </summary>
        protected bool ExpandDetailException { get; }

        /// <inheritdoc />
        public Exception Process(Exception exception) =>
            this.contextEvaluator.Evaluate(DBSessionMode.Write, context => this.Process(exception, context));

        private Exception Process(Exception exception, IConfigurationBLLContext context)
        {
            var evaluateException = this.ToEvaluateException(exception, context);

            this.Save(evaluateException.ExpandedBaseException, context);

            return this.apiControllerPostProcessExceptionService.Process(evaluateException, context);
        }

        private EvaluateException ToEvaluateException(Exception exception, IConfigurationBLLContext context)
        {
            if (exception is EvaluateException evaluateException)
            {
                return evaluateException;
            }

            return new EvaluateException(exception, context.ExceptionService.Process(exception));
        }

        private void Save([NotNull] Exception exception, IConfigurationBLLContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            this.TrySaveExceptionMessage(exception, context);
        }


        private void TrySaveExceptionMessage(Exception exception, IConfigurationBLLContext context)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            try
            {
                context.ExceptionService.Save(exception);
            }
            catch
            {
            }
        }
    }
}
