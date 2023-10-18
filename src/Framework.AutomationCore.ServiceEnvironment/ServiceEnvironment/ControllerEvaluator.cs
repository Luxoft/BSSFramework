using System.Linq.Expressions;

using Automation.ServiceEnvironment.Services;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public class ControllerEvaluator<TController>
        where TController : ControllerBase
{
    private readonly IServiceProvider rootServiceProvider;

    private readonly string customPrincipalName;

    public ControllerEvaluator(IServiceProvider rootServiceProvider)
            : this(rootServiceProvider, null)
    {
    }

    private ControllerEvaluator(IServiceProvider rootServiceProvider, string customPrincipalName)
    {
        this.rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
        this.customPrincipalName = customPrincipalName;
    }

    public void Evaluate(Expression<Action<TController>> actionExpr)
    {
        this.InternalEvaluateAsync<object>(actionExpr, async c =>
        {
            actionExpr.Eval(c);
            return default;
        }).GetAwaiter().GetResult();
    }

    public T Evaluate<T>(Expression<Func<TController, T>> funcExpr)
    {
        if (TaskResultHelper<T>.IsTask)
        {
            throw new Exception($"For Task result use {nameof(EvaluateAsync)} method");
        }

        return this.InternalEvaluateAsync(funcExpr, c => Task.FromResult(funcExpr.Eval(c))).GetAwaiter().GetResult();
    }

    public async Task EvaluateAsync(Expression<Func<TController, Task>> actionExpr)
    {
        await this.InternalEvaluateAsync<object>(actionExpr, async c =>
        {
            await actionExpr.Eval(c);
            return default;
        });
    }

    public async Task<T> EvaluateAsync<T>(Expression<Func<TController, Task<T>>> funcExpr)
    {
        return await this.InternalEvaluateAsync(funcExpr, funcExpr.Compile(LambdaCompileCache.Default));
    }

    private async Task<T> InternalEvaluateAsync<T>(LambdaExpression invokeExpr, Func<TController, Task<T>> func)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        var c = new DefaultHttpContext { RequestServices = scope.ServiceProvider };

        await new WebApiInvoker(c, context => InvokeController(context, func))
              .WithMiddleware(next => new ImpersonateMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, this.customPrincipalName))
              .WithMiddleware(next => new TryProcessDbSessionMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IDBSessionManager>(), httpContext.RequestServices.GetRequiredService<IWebApiDBSessionModeResolver>()))
              .WithMiddleware(next => new InitCurrentMethodMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, invokeExpr))
              .WithMiddleware(next => new WebApiExceptionExpanderMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IWebApiExceptionExpander>()))
              .Invoke();

        return (T)c.Items["Result"];
    }

    private static async Task InvokeController<T>(HttpContext context, Func<TController, Task<T>> func)
    {
        var controller = context.RequestServices.GetRequiredService<TController>();

        controller.ControllerContext.HttpContext = context;

        var res = await func(controller);

        context.Items["Result"] = res;
    }

    public ControllerEvaluator<TController> WithImpersonate(string newCustomPrincipalName)
    {
        return new ControllerEvaluator<TController>(this.rootServiceProvider, newCustomPrincipalName);
    }

    private class ImpersonateMiddleware
    {
        private readonly RequestDelegate next;

        public ImpersonateMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, string customPrincipalName)
        {
            if (customPrincipalName == null)
            {
                await this.next(context);
            }
            else
            {
                await context.RequestServices.GetRequiredService<IntegrationTestUserAuthenticationService>().WithImpersonateAsync(customPrincipalName, async () =>
                {
                    await this.next(context);
                    return default(object);
                });
            }
        }
    }

    private class InitCurrentMethodMiddleware
    {
        private readonly RequestDelegate next;

        public InitCurrentMethodMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, LambdaExpression invokeExpr)
        {
            var currentMethod = invokeExpr.UpdateBodyBase(ExpandConstVisitor.Value)
                                          .TryGetStartMethodInfo()
                                          .FromMaybe("Current controller method can't be extracted");

            context.RequestServices.GetRequiredService<TestWebApiCurrentMethodResolver>().SetCurrentMethod(currentMethod);

            await this.next(context);
        }
    }

    private class WebApiInvoker
    {
        private readonly HttpContext context;

        private readonly RequestDelegate next;

        public WebApiInvoker(HttpContext context, RequestDelegate next)
        {
            this.context = context;
            this.next = next;
        }

        public WebApiInvoker WithMiddleware<TMiddleware>(Func<RequestDelegate, TMiddleware> createFunc, Func<TMiddleware, HttpContext, Task> invokeDelegate)
        {
            return new WebApiInvoker(this.context, c => invokeDelegate(createFunc(this.next), c));
        }

        public async Task Invoke()
        {
            await this.next(this.context);
        }
    }

    private static class TaskResultHelper<TResult>
    {
        public static readonly bool IsTask = typeof(Task).IsAssignableFrom(typeof(TResult));
    }
}
