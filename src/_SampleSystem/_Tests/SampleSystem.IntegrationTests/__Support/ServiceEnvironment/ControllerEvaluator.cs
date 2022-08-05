using System;
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

    public async Task<T> EvaluateAsync<T>(Func<TController, Task<T>> func)
    {
        await using var scope = this.rootServiceProvider.CreateAsyncScope();

        return await new WebApiInvoker<T>(new DefaultHttpContext { RequestServices = scope.ServiceProvider }, context => InvokeController(context, func))
                .WithMidlleware(next => new ImpersonateMiddleware<T>(next), (middleware, httpContext) => middleware.Invoke(httpContext, this.customPrincipalName))
                .WithMidlleware(next => new TryProcessDbSessionMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext))
                .WithMidlleware(next => new WebApiExceptionExpanderMiddleware(next), (middleware, httpContext) => middleware.Invoke(httpContext, httpContext.RequestServices.GetRequiredService<IWebApiExceptionExpander>()))
                .Invoke();
    }

    private static Task<T> InvokeController<T>(HttpContext context, Func<TController, Task<T>> func)
    {
        var controller = context.RequestServices.GetRequiredService<TController>();

        (controller as IApiControllerBase).Maybe(c => c.ServiceProvider = context.RequestServices);

        return func(controller);
    }

    private async Task<T> InternalEvaluateAsync<T>(IServiceProvider scopeServiceProvider, Func<TController, Task<T>> func)
    {
        var controller = scopeServiceProvider.GetRequiredService<TController>();

        (controller as IApiControllerBase).Maybe(c => c.ServiceProvider = scopeServiceProvider);

        if (this.customPrincipalName == null)
        {
            return await func(controller);
        }
        else
        {
            return await scopeServiceProvider.GetRequiredService<IntegrationTestDefaultUserAuthenticationService>().WithImpersonateAsync(this.customPrincipalName, async () => await func(controller));
        }
    }



    public T Evaluate<T>(Func<TController, T> func)
    {
        return this.EvaluateAsync(c => Task.FromResult(func(c))).GetAwaiter().GetResult();
    }

    public async Task EvaluateAsync(Func<TController, Task> action)
    {
        await this.EvaluateAsync<object>(async c =>
        {
            await action(c);
            return default;
        });
    }

    public void Evaluate(Action<TController> action)
    {
        this.Evaluate(c =>
                      {
                          action(c);
                          return default(object);
                      });
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

        public WebApiInvoker<T> WithMidlleware<TMiddleware>(Func<RequestDelegate, TMiddleware> createFunc, Func<TMiddleware, HttpContext, Task> invokeDelegate)
        {
            return new WebApiInvoker<T>(this.context, c => invokeDelegate(createFunc(this.next), c));
        }

        public Task<T> Invoke()
        {
            return (Task<T>)this.next(this.context);
        }
    }
}
