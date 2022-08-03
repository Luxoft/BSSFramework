using System;
using System.Threading.Tasks;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.WebApiNetCore;
using Framework.Exceptions;

using JetBrains.Annotations;

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

    public T Evaluate<T>(Func<TController, T> func)
    {
        return this.EvaluateAsync(c => Task.FromResult(func(c))).Result;
    }

    public async Task<T> EvaluateAsync<T>(Func<TController, Task<T>> func)
    {
        try
        {
            using var scope = this.rootServiceProvider.CreateScope();

            var scopeServiceProvider = scope.ServiceProvider;

            try
            {
                return await this.InternalEvaluateAsync(scopeServiceProvider, func);
            }
            catch (Exception baseException)
            {
                scopeServiceProvider.TryFaultDbSession();

                var expandedBaseException = scopeServiceProvider.GetRequiredService<IConfigurationBLLContext>().ExceptionService.Process(baseException, true);

                throw new EvaluateException(baseException, expandedBaseException);
            }
            finally
            {
                scopeServiceProvider.TryCloseDbSession();
            }
        }
        catch (Exception ex)
        {
            var ep = this.rootServiceProvider.GetRequiredService<IRootExceptionService>();

            var processEx = ep.Process(ex);

            if (processEx != ex)
            {
                throw processEx;
            }
            else
            {
                throw;
            }
        }
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
}
