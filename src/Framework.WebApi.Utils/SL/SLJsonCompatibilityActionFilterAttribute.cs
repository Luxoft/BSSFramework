using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.WebApi.Utils.SL;

public class SLJsonCompatibilityActionFilterAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionDescriptor.Parameters.Any())
        {
            var service = context.HttpContext.RequestServices.GetRequiredService<ISlJsonCompatibilitySerializer>();

            await service.DeserializeParameters(context);
        }

        await base.OnActionExecutionAsync(context, next);
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var service = context.HttpContext.RequestServices.GetRequiredService<ISlJsonCompatibilitySerializer>();

        if (context.Exception != null)
        {
            context.Result = new JsonResult(new { FaultData = context.Exception.Message });
            context.Exception = null;
        }
        else
        {
            context.Result = context.Result switch
            {
                ObjectResult objectResult => service.CreateJsonResult(context, objectResult.Value),
                EmptyResult => service.CreateJsonResult(context, new object()),
                _ => throw new NotSupportedException()
            };
        }

        base.OnActionExecuted(context);
    }
}
