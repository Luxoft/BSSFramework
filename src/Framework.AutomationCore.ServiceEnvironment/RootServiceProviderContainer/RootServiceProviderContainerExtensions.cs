using System;
using System.Linq.Expressions;

using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public static class RootServiceProviderContainerExtensions
{
    public static IDatabaseContext GetDatabaseContext(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IDatabaseContext>();
    }

    public static ConfigUtil GetConfigUtil(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<ConfigUtil>();
    }

    public static IDateTimeService GetDateTimeService(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IDateTimeService>();
    }

    public static void SetCurrentDateTime(this IRootServiceProviderContainer rootServiceProviderContainer, DateTime newDateTime)
    {
        rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IntegrationTestDateTimeService>().SetCurrentDateTime(newDateTime);
    }

    public static TResult EvaluateController<TController, TResult>(
            this IRootServiceProviderContainer controllerEvaluator,
            Expression<Func<TController, TResult>> func)
            where TController : ControllerBase
    {
        return controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(func);
    }

    public static void EvaluateController<TController>(
            this IRootServiceProviderContainer controllerEvaluator,
            Expression<Action<TController>> action)
            where TController : ControllerBase
    {
        controllerEvaluator.RootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>().Evaluate(action);
    }
}
