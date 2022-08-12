using System;
using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private readonly Lazy<MethodInfo> lazyCurrentMethod;

    public WebApiCurrentMethodResolver(HttpContext httpContext)
    {
        this.lazyCurrentMethod = new Lazy<MethodInfo>(() =>
             httpContext
                .GetEndpoint()
                .Metadata
                .GetMetadata<ControllerActionDescriptor>()
                .MethodInfo);
    }

    public MethodInfo CurrentMethod => this.lazyCurrentMethod.Value;
}
