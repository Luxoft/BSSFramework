using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Framework.WebApi.Utils.SL
{
    public class SLJsonCompatibilityActionFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any())
            {
                var jsonDoc = (JsonDocument)context.HttpContext.Items[nameof(JsonDocument)];

                foreach (var parameter in context.ActionDescriptor.Parameters)
                {
                    var jsonPropValue = jsonDoc.RootElement.GetProperty(parameter.Name);

                    context.ActionArguments[parameter.Name] = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), parameter.ParameterType);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                context.Result = new JsonResult(new { FaultData = context.Exception.Message });

                context.Exception = null;
            }
            else if (context.Result is ObjectResult objectResult)
            {
                context.Result = CreateJsonResult(context, objectResult.Value);
            }
            else if (context.Result is EmptyResult)
            {
                context.Result = CreateJsonResult(context, new object());
            }
            else
            {
                throw new NotImplementedException();
            }

            base.OnActionExecuted(context);
        }

        private static JsonResult CreateJsonResult(ActionExecutedContext context, object value)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            IDictionary<string, object> e = new ExpandoObject();
            e.Add(actionDescriptor.ActionName + "Result", value);

            return new JsonResult(e);
        }
    }
}
