using System;

using Framework.Core.Services;
using Framework.DomainDriven.WebApiNetCore;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AttachmentsSampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public static class ControllerEvaluatorContainerExtensions
{
    public static TResult EvaluateController<TController, TResult>(this IControllerEvaluatorContainer controllerEvaluator, Func<TController, TResult> func)
            where TController : ControllerBase, IApiControllerBase
    {
        return controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(func);
    }

    public static void EvaluateController<TController>(this IControllerEvaluatorContainer controllerEvaluator, Action<TController> action)
            where TController : ControllerBase, IApiControllerBase
    {
        controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(action);
    }

    public static string GetCurrentUserName(this IControllerEvaluatorContainer controllerEvaluatorContainer)
    {
        return controllerEvaluatorContainer.RootServiceProvider.GetRequiredService<IUserAuthenticationService>().GetUserName();
    }
}
