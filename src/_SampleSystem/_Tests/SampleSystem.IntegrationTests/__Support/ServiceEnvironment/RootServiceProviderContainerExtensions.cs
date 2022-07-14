using System;

using Framework.Core.Services;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SampleSystem.BLL;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public static class RootServiceProviderContainerExtensions
{
    public static TResult EvaluateController<TController, TResult>(this IRootServiceProviderContainer controllerEvaluator, Func<TController, TResult> func)
            where TController : ControllerBase, IApiControllerBase
    {
        return controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(func);
    }

    public static void EvaluateController<TController>(this IRootServiceProviderContainer controllerEvaluator, Action<TController> action)
            where TController : ControllerBase, IApiControllerBase
    {
        controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(action);
    }

    public static IContextEvaluator<ISampleSystemBLLContext> GetContextEvaluator(this IRootServiceProviderContainer controllerEvaluatorContainer)
    {
        return controllerEvaluatorContainer.RootServiceProvider.GetRequiredService<IContextEvaluator<ISampleSystemBLLContext>>();
    }

    public static string GetCurrentUserName(this IRootServiceProviderContainer controllerEvaluatorContainer)
    {
        return controllerEvaluatorContainer.RootServiceProvider.GetRequiredService<IUserAuthenticationService>().GetUserName();
    }
}
