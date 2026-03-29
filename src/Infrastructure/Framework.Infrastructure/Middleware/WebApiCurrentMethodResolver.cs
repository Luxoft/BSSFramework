using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Framework.Infrastructure.Middleware;

public class WebApiCurrentMethodResolver(IHttpContextAccessor httpContextAccessor) : IWebApiCurrentMethodResolver
{
    public MethodInfo? TryGetCurrentMethod()
    {
        var endPoint = httpContextAccessor.HttpContext?.GetEndpoint();

        return endPoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;
    }
}
