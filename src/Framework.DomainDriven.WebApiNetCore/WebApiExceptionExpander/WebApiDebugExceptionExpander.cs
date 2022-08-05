using System;

using Framework.Notification;

namespace Framework.DomainDriven.WebApiNetCore
{
    /// <inheritdoc />
    public class WebApiDebugExceptionExpander : IWebApiExceptionExpander
    {
        private readonly IExceptionExpander exceptionExpander;

        public WebApiDebugExceptionExpander(IExceptionExpander exceptionExpander)
        {
            this.exceptionExpander = exceptionExpander ?? throw new ArgumentNullException(nameof(exceptionExpander));
        }

        /// <inheritdoc />
        public Exception Process(Exception baseException)
        {
            var expandedException = this.exceptionExpander.Process(baseException);

            if (expandedException == baseException)
            {
                return baseException;
            }

            return expandedException;
        }
    }
}
