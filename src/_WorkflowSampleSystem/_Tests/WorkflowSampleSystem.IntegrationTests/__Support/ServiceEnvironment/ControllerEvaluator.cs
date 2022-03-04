using System;

using Framework.DomainDriven.WebApiNetCore;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using WorkflowSampleSystem.IntegrationTests.__Support.TestData;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

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
        using var scope = this.serviceProvider.CreateScope();

        var controller = scope.ServiceProvider.GetRequiredService<TController>();

        controller.ServiceProvider = scope.ServiceProvider;
        controller.PrincipalName = this.principalName;

        return func(controller);
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
