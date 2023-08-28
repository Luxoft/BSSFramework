using System.Net;
using System.Net.Mime;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Framework.WebApi.Utils;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public virtual async Task Invoke(HttpContext context)
    {
        var response = context.Response;

        var originBody = response.Body;
        using var newBody = new MemoryStream();
        response.Body = newBody;

        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(context, ex, newBody);
        }
        finally
        {
            newBody.Seek(0, SeekOrigin.Begin);
            await newBody.CopyToAsync(originBody, context.RequestAborted);
            response.Body = originBody;
        }
    }

    protected virtual async Task HandleExceptionAsync(HttpContext context, Exception exception, MemoryStream newBody)
    {
        var errorInfo = this.GetErrorInfo(exception);
        context.Response.ContentType = errorInfo.contentType;
        context.Response.StatusCode = errorInfo.statucCode;

        var errorData = JsonConvert.SerializeObject(errorInfo.errorMessage);

        newBody.SetLength(0);
        await using var writer = new StreamWriter(newBody, leaveOpen: true);
        await writer.WriteAsync(errorData);
        await writer.FlushAsync();
        context.Response.ContentLength = newBody.Length;
    }

    protected virtual (string contentType, int statucCode, string errorMessage) GetErrorInfo(Exception exception)
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

        return (MediaTypeNames.Application.Json, code, exceptionMessage);
    }
}
