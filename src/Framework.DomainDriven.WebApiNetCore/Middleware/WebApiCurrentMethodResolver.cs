using System;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private readonly IHttpContextAccessor httpContextAccessor;

    public WebApiCurrentMethodResolver([NotNull] IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public MethodInfo GetCurrentMethod()
    {
        var endPoint = this.httpContextAccessor?.HttpContext?.GetEndpoint();

        return endPoint?.Metadata.GetMetadata<ControllerActionDescriptor>()?.MethodInfo;
    }
}
