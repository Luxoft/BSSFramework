using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.WebApiNetCore;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class ControllerEvaluator<TController>
        where TController : ControllerBase
{
    private readonly IServiceProvider rootServiceProvider;

    private readonly string customPrincipalName;

    public ControllerEvaluator([NotNull] IServiceProvider rootServiceProvider)
            : this(rootServiceProvider, null)
    {
    }

    private ControllerEvaluator([NotNull] IServiceProvider rootServiceProvider, string customPrincipalName)
    {
        this.rootServiceProvider = rootServiceProvider ?? throw new ArgumentNullException(nameof(rootServiceProvider));
        this.customPrincipalName = customPrincipalName;
    }

    public void Evaluate(Expression<Action<TController>> actionExpr)
    {
        this.InternalEvaluateAsync(actionExpr, async c =>
        {
            actionExpr.Eval(c);
            return default(object);
        }).GetAwaiter().GetResult();
    }

    public T Evaluate<T>(Expression<Func<TController, T>> funcExpr)
    {
        return this.InternalEvaluateAsync(funcExpr, c => Task.FromResult(funcExpr.Eval(c))).GetAwaiter().GetResult();
    }

    public Task EvaluateAsync(Expression<Func<TController, Task>> actionExpr)
    {
        return this.InternalEvaluateAsync<object>(actionExpr, async c =>
        {
            await actionExpr.Eval(c);
            return default;
        });
    }

    public Task<T> EvaluateAsync<T>(Expression<Func<TController, Task<T>>> funcExpr)
    {
        return this.InternalEvaluateAsync(funcExpr, funcExpr.Compile(LambdaCompileCache.Default));
    }

    private async Task<T> InternalEvaluateAsync<T>(LambdaExpression invokeExpr, Func<TController, Task<T>> func)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();


        scope.ServiceProvider
             .GetRequiredService<IntegrationTestsWebApiCurrentMethodResolver>()
             .CurrentMethod = invokeExpr.UpdateBodyBase(ExpandConstVisitor.Value)
                                        .TryGetStartMethodInfo()
                                        .FromMaybe("method can't be extracted");

        return await new WebApiInvoker<T>(new DefaultHttpContext { RequestServices = scope.ServiceProvider }, context => InvokeController(context, func))
                .WithMiddleware(next => new ImpersonateMiddleware<T>(next), (middleware, httpContext) => middleware.Invoke(httpContext, this.customPrincipalName))
                .WithMiddleware(next => new TryProcessDbSessionMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IWebApiDBSessionModeResolver>()))
                .WithMiddleware(next => new WebApiExceptionExpanderMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IWebApiExceptionExpander>()))
                .Invoke();
    }

    private static Task<T> InvokeController<T>(HttpContext context, Func<TController, Task<T>> func)
    {
        var controller = context.RequestServices.GetRequiredService<TController>();

        controller.ControllerContext.HttpContext = context;

        return func(controller);
    }

    public ControllerEvaluator<TController> WithImpersonate([CanBeNull] string newCustomPrincipalName)
    {
        return new ControllerEvaluator<TController>(this.rootServiceProvider, newCustomPrincipalName);
    }

    public ControllerEvaluator<TController> WithIntegrationImpersonate()
    {
        return this.WithImpersonate(DefaultConstants.INTEGRATION_USER);
    }

    private class ImpersonateMiddleware<T>
    {
        private readonly RequestDelegate next;

        public ImpersonateMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context, string customPrincipalName)
        {
            if (customPrincipalName == null)
            {
                return this.next(context);
            }
            else
            {
                return context.RequestServices.GetRequiredService<IntegrationTestDefaultUserAuthenticationService>().WithImpersonateAsync(customPrincipalName, () => (Task<T>)this.next(context));
            }
        }
    }

    private class WebApiInvoker<T>
    {
        private readonly HttpContext context;

        private readonly RequestDelegate next;

        public WebApiInvoker(HttpContext context, RequestDelegate next)
        {
            this.context = context;
            this.next = next;
        }

        public WebApiInvoker<T> WithMiddleware<TMiddleware>(Func<RequestDelegate, TMiddleware> createFunc, Func<TMiddleware, HttpContext, Task> invokeDelegate)
        {
            return new WebApiInvoker<T>(this.context, c => invokeDelegate(createFunc(this.next), c));
        }

        public Task<T> Invoke()
        {
            return (Task<T>)this.next(this.context);
        }
    }
}
