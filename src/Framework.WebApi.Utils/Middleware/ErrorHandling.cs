using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Framework.WebApi.Utils;

internal sealed class ErrorHandling
{
    private readonly RequestDelegate next;

    public ErrorHandling(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = (int)HttpStatusCode.InternalServerError;
        var exceptionMessage = exception.Message;

        switch (exception)
        {
            case WebException e:
                code = (int)e.Status;
                break;
            default:
                exceptionMessage = exception.InnerException?.Message ?? exceptionMessage;
                break;
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = code;

        return context.Response.WriteAsync(JsonConvert.SerializeObject(exceptionMessage));
    }
}
