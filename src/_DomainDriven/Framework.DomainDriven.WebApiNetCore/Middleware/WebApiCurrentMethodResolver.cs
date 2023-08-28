using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public WebApiCurrentMethodResolver(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public MethodInfo GetCurrentMethod()
    {
        var endPoint = this.httpContextAccessor?.HttpContext?.GetEndpoint();

        return endPoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;
    }
}
