using System;
using System.Linq;
using System.Reflection;

using Framework.Core;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

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
