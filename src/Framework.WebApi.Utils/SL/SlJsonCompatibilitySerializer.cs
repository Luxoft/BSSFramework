using System.Dynamic;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.WebApi.Utils.SL;

public class SlJsonCompatibilitySerializer : ISlJsonCompatibilitySerializer
{
    public async Task DeserializeParameters(ActionExecutingContext context)
    {
        var jsonDoc = (JsonDocument)context.HttpContext.Items[nameof(JsonDocument)];

        foreach (var parameter in context.ActionDescriptor.Parameters)
        {
            if (parameter.ParameterType != typeof(CancellationToken))
            {
                var jsonPropValue = jsonDoc.RootElement.GetProperty(parameter.Name);

                context.ActionArguments[parameter.Name] = JsonSerializer.Deserialize(jsonPropValue.GetRawText(), parameter.ParameterType);
            }
        }
    }

    public JsonResult CreateJsonResult(ActionExecutedContext context, object value)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

        IDictionary<string, object> e = new ExpandoObject();
        e.Add(actionDescriptor.ActionName + "Result", value);

        return new JsonResult(e);
    }
}
