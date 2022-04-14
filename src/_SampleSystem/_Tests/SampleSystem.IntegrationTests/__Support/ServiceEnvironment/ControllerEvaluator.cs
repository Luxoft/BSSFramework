using System;
using System.Threading.Tasks;

using Framework.DomainDriven.WebApiNetCore;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.IntegrationTests.__Support.TestData;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class ControllerEvaluator<TController>
        where TController : ControllerBase, IApiControllerBase
{
    private readonly IServiceProvider serviceProvider;

    private readonly string principalName;

    public ControllerEvaluator([NotNull] IServiceProvider serviceProvider)
            : this(serviceProvider, null)
    {
    }

    private ControllerEvaluator([NotNull] IServiceProvider serviceProvider, string principalName)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        this.principalName = principalName;
    }

    public T Evaluate<T>(Func<TController, T> func)
    {
        return this.EvaluateAsync(async c => func(c)).Result;
    }

    public async Task<T> EvaluateAsync<T>(Func<TController, Task<T>> func)
    {
        using var scope = this.serviceProvider.CreateScope();

        var controller = scope.ServiceProvider.GetRequiredService<TController>();

        controller.ServiceProvider = scope.ServiceProvider;

        if (this.principalName == null)
        {
            return await func(controller);
        }
        else
        {
            return await IntegrationTestsUserAuthenticationService.Instance.ImpersonateAsync(this.principalName, async () => await func(controller));
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

    public ControllerEvaluator<TController> WithImpersonate([NotNull] string principalName)
    {
        if (principalName == null) throw new ArgumentNullException(nameof(principalName));

        return new ControllerEvaluator<TController>(this.serviceProvider, principalName);
    }

    public ControllerEvaluator<TController> WithIntegrationImpersonate()
    {
        return this.WithImpersonate(DefaultConstants.INTEGRATION_USER);
    }
}
