﻿using Microsoft.AspNetCore.Builder;

namespace Framework.DomainDriven.WebApiNetCore;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseTryProcessDbSession(this IApplicationBuilder builder) => builder.UseMiddleware<TryProcessDbSessionMiddleware>();

    public static IApplicationBuilder UseWebApiExceptionExpander(this IApplicationBuilder builder) => builder.UseMiddleware<WebApiExceptionExpanderMiddleware>();
}
