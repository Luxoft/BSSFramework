using System.Linq.Expressions;

using Anch.Core;
using Anch.Core.Visitor;

using Framework.Core;
using Framework.Database;
using Framework.Infrastructure.Middleware;
using Framework.Infrastructure.WebApiExceptionExpander;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using Anch.SecuritySystem;
using Anch.SecuritySystem.Services;
using Anch.SecuritySystem.Testing;

namespace Framework.AutomationCore.ServiceEnvironment;

public class ControllerEvaluator<TController>(IServiceProvider rootServiceProvider, UserCredential? customUserCredential = null)
    where TController : ControllerBase
{
    public void Evaluate(Expression<Action<TController>> actionExpr) =>
        this.InternalEvaluateAsync<object?>(actionExpr, async c =>
        {
            actionExpr.Compile().Invoke(c);
            return null;
        }).GetAwaiter().GetResult();

    public T Evaluate<T>(Expression<Func<TController, T>> funcExpr)
    {
        TaskResultHelper<T>.TypeIsNotTaskValidate();

        return this.InternalEvaluateAsync(funcExpr, async c => funcExpr.Compile().Invoke(c)).GetAwaiter().GetResult();
    }

    public async Task EvaluateAsync(Expression<Func<TController, Task>> actionExpr) =>
        await this.InternalEvaluateAsync<object?>(actionExpr, async c =>
        {
            await actionExpr.Compile().Invoke(c);
            return null;
        });

    public async Task<T> EvaluateAsync<T>(Expression<Func<TController, Task<T>>> funcExpr) => await this.InternalEvaluateAsync(funcExpr, funcExpr.Compile());

    private async Task<T> InternalEvaluateAsync<T>(LambdaExpression invokeExpr, Func<TController, Task<T>> func)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var c = new DefaultHttpContext { RequestServices = scope.ServiceProvider };

        await new WebApiInvoker(c, context => InvokeController(context, func))
              .WithMiddleware(next => new ImpersonateMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, customUserCredential))
              .WithMiddleware(next => new TryProcessDbSessionMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IDBSessionManager>(), httpContext.RequestServices.GetRequiredService<IWebApiCurrentDBSessionModeResolver>()))
              .WithMiddleware(next => new InitCurrentMethodMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, invokeExpr))
              .WithMiddleware(next => new WebApiExceptionExpanderMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IWebApiExceptionExpander>()))
              .Invoke();

        return (T)c.Items["Result"]!;
    }

    private static async Task InvokeController<T>(HttpContext context, Func<TController, Task<T>> func)
    {
        var controller = context.RequestServices.GetRequiredService<TController>();

        controller.ControllerContext.HttpContext = context;

        var res = await func(controller);

        context.Items["Result"] = res;
    }

    public ControllerEvaluator<TController> WithImpersonate(UserCredential newCustomUserCredential) => new(rootServiceProvider, newCustomUserCredential);

    private class ImpersonateMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, UserCredential? customUserCredential)
        {
            if (customUserCredential == null)
            {
                await next(context);
            }
            else
            {
                await context.RequestServices
                             .GetRequiredService<IRootImpersonateService>()
                             .WithImpersonateAsync(customUserCredential, async () => await next(context));
            }
        }
    }

    private class InitCurrentMethodMiddleware(RequestDelegate next)
    {
        public async Task Invoke(HttpContext context, LambdaExpression invokeExpr)
        {
            var currentMethod = invokeExpr.UpdateBodyBase(ExpandConstVisitor.Value)
                                          .TryGetStartMethodInfo()
                                          .FromMaybe(() => "Current controller method can't be extracted");

            context.RequestServices.GetRequiredService<TestWebApiCurrentMethodResolver>().SetCurrentMethod(currentMethod);

            await next(context);
        }
    }

    private class WebApiInvoker(HttpContext context, RequestDelegate next)
    {
        public WebApiInvoker WithMiddleware<TMiddleware>(Func<RequestDelegate, TMiddleware> createFunc, Func<TMiddleware, HttpContext, Task> invokeDelegate) => new(context, c => invokeDelegate(createFunc(next), c));

        public async Task Invoke() => await next(context);
    }
}
