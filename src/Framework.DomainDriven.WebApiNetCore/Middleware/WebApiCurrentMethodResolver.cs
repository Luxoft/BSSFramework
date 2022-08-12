using System.Reflection;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Framework.DomainDriven.WebApiNetCore;

public class WebApiCurrentMethodResolver : IWebApiCurrentMethodResolver
{
    private readonly HttpContext httpContext;

    public WebApiCurrentMethodResolver(HttpContext httpContext)
    {
        this.httpContext = httpContext;
    }

    public MethodInfo CurrentMethod  =>

            this.httpContext
                .GetEndpoint()
                .Metadata
                .GetMetadata<ControllerActionDescriptor>()
                .MethodInfo;


}
