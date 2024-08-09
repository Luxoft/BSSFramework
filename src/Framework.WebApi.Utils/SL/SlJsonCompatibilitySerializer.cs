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
                var actualElement = jsonDoc.RootElement.TryGetProperty(parameter.Name, out var jsonPropValue) ? jsonPropValue : jsonDoc.RootElement;

                var rawText = actualElement.GetRawText();

                context.ActionArguments[parameter.Name] = this.DeserializeParameter(rawText, parameter.ParameterType);
            }
        }
    }

    public JsonResult CreateJsonResult(ActionExecutedContext context, object value)
    {
        var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

        IDictionary<string, object> e = new System.Dynamic.ExpandoObject();
        e.Add(actionDescriptor.ActionName + "Result", value);

        return this.SerializeResult(e);
    }

    protected virtual object DeserializeParameter(string rawText, Type parameterType)
    {
        return JsonSerializer.Deserialize(rawText, parameterType);
    }

    protected virtual JsonResult SerializeResult(IDictionary<string, object> obj)
    {
        return new JsonResult(obj);
    }
}
