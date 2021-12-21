using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Framework.WebApi.Utils
{
    public sealed class LogActionAttribute : ActionFilterAttribute
    {
        private readonly string key;

        private readonly ILogger logger;

        private IDictionary<string, object> actionArguments;

        public LogActionAttribute(ILogger<LogActionAttribute> logger, string key = "ActionWatch")
        {
            this.logger = logger;
            this.key = key;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.actionArguments = context.ActionArguments;
            context.ActionDescriptor.Properties[this.key] = Stopwatch.StartNew();

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            var watch = (Stopwatch)context.ActionDescriptor.Properties[this.key];
            watch.Stop();

            this.LogAction(context, watch);
        }

        private void LogAction(ActionExecutedContext actionExecutedContext, Stopwatch watch)
        {
            using (this.logger.BeginScope(this.actionArguments))
            {
                switch (actionExecutedContext.Exception)
                {
                    case null:
                        this.logger.LogInformation("Request successfully completed in {TotalTime} milliseconds", watch.ElapsedMilliseconds);
                        break;
                    default:
                        this.logger.LogError(actionExecutedContext.Exception, "Request completed with error");
                        break;
                }
            }
        }
    }
}
