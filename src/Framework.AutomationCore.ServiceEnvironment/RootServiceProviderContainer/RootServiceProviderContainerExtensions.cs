using System.Linq.Expressions;

using Automation.ServiceEnvironment.Services;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

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

    public static TimeProvider GetTimeProvider(this IRootServiceProviderContainer rootServiceProviderContainer)
    {
        return rootServiceProviderContainer.RootServiceProvider.GetRequiredService<TimeProvider>();
    }

    public static void SetCurrentDateTime(this IRootServiceProviderContainer rootServiceProviderContainer, DateTime newDateTime)
    {
        rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IntegrationTestTimeProvider>().SetCurrentDateTime(newDateTime);
    }

    public static TResult EvaluateController<TController, TResult>(
            this IRootServiceProviderContainer rootServiceProviderContainer,
            Expression<Func<TController, TResult>> func)
            where TController : ControllerBase
    {
        return rootServiceProviderContainer.RootServiceProvider.GetDefaultControllerEvaluator<TController>().Evaluate(func);
    }

    public static void EvaluateController<TController>(
            this IRootServiceProviderContainer rootServiceProviderContainer,
            Expression<Action<TController>> action)
            where TController : ControllerBase
    {
        rootServiceProviderContainer.RootServiceProvider.GetDefaultControllerEvaluator<TController>().Evaluate(action);
    }
}
