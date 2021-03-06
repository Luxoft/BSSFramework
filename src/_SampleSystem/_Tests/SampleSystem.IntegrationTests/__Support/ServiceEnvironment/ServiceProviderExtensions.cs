using System;

using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public static class ServiceProviderExtensions
{
    public static ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(this IServiceProvider serviceProvider, string principalName = null)
            where TController : ControllerBase
    {
        var controllerEvaluator = serviceProvider.GetRequiredService<ControllerEvaluator<TController>>();

        return principalName == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(principalName);
    }
}

public class TestDebugModeManager : IDebugModeManager
{
    public bool IsDebugMode { get; set; } = true;
}
