using System.Linq.Expressions;

using Automation.ServiceEnvironment.Services;

using CommonFramework;
using CommonFramework.Visitor;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore;
using SecuritySystem.Credential;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class ControllerEvaluator<TController>(IServiceProvider rootServiceProvider, UserCredential? customUserCredential = null)
    where TController : ControllerBase
{
    public void Evaluate(Expression<Action<TController>> actionExpr)
    {
        this.InternalEvaluateAsync<object?>(actionExpr, async c =>
        {
            actionExpr.Compile().Invoke(c);
            return default;
        }).GetAwaiter().GetResult();
    }

    public T Evaluate<T>(Expression<Func<TController, T>> funcExpr)
    {
        TaskResultHelper<T>.TypeIsNotTaskValidate();

        return this.InternalEvaluateAsync(funcExpr, async c => funcExpr.Compile().Invoke(c)).GetAwaiter().GetResult();
    }

    public async Task EvaluateAsync(Expression<Func<TController, Task>> actionExpr)
    {
        await this.InternalEvaluateAsync<object?>(actionExpr, async c =>
        {
            await actionExpr.Compile().Invoke(c);
            return default;
        });
    }

    public async Task<T> EvaluateAsync<T>(Expression<Func<TController, Task<T>>> funcExpr)
    {
        return await this.InternalEvaluateAsync(funcExpr, funcExpr.Compile());
    }

    private async Task<T> InternalEvaluateAsync<T>(LambdaExpression invokeExpr, Func<TController, Task<T>> func)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var c = new DefaultHttpContext { RequestServices = scope.ServiceProvider };

        await new WebApiInvoker(c, context => InvokeController(context, func))
              .WithMiddleware(next => new ImpersonateMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, customUserCredential))
              .WithMiddleware(next => new TryProcessDbSessionMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IDBSessionManager>(), httpContext.RequestServices.GetRequiredService<IWebApiDBSessionModeResolver>()))
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

    public ControllerEvaluator<TController> WithImpersonate(UserCredential newCustomUserCredential)
    {
        return new ControllerEvaluator<TController>(rootServiceProvider, newCustomUserCredential);
    }

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
                             .GetRequiredService<IIntegrationTestUserAuthenticationService>()
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
        public WebApiInvoker WithMiddleware<TMiddleware>(Func<RequestDelegate, TMiddleware> createFunc, Func<TMiddleware, HttpContext, Task> invokeDelegate)
        {
            return new WebApiInvoker(context, c => invokeDelegate(createFunc(next), c));
        }

        public async Task Invoke()
        {
            await next(context);
        }
    }
}
