using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Framework.WebApi.Utils.SL;

public interface ISlJsonCompatibilitySerializer
{
    Task DeserializeParameters(ActionExecutingContext context);

    JsonResult CreateJsonResult(ActionExecutedContext context, object value);
}
