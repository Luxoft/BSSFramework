using System;
using System.Net;
using System.Net.Mime;

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
        var response = context.Response;

        var originBody = response.Body;
        var newBody = new MemoryStream();
        response.Body = newBody;

        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, newBody);
        }

        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originBody);
        response.Body = originBody;
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception, MemoryStream stream)
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

        var response = context.Response;

        response.ContentType = MediaTypeNames.Application.Json;
        response.StatusCode = code;

        stream.SetLength(0);

        var modifiedResponse = JsonConvert.SerializeObject(exceptionMessage);

        await using var writer = new StreamWriter(stream, leaveOpen: true);
        await writer.WriteAsync(modifiedResponse);
        await writer.FlushAsync();
        response.ContentLength = stream.Length;
    }
}
