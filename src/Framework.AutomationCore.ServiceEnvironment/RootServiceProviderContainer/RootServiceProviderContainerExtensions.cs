using System.Linq.Expressions;

using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment;

public static class RootServiceProviderContainerExtensions
{
    extension(IRootServiceProviderContainer rootServiceProviderContainer)
    {
        public IDatabaseContext GetDatabaseContext() => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IDatabaseContext>();

        public AutomationFrameworkSettings GetAutomationFrameworkSettings() => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IOptions<AutomationFrameworkSettings>>().Value;

        public TimeProvider GetTimeProvider() => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<TimeProvider>();

        public void SetCurrentDateTime(DateTime newDateTime) => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IntegrationTestTimeProvider>().SetCurrentDateTime(newDateTime);

        public TResult EvaluateController<TController, TResult>(Expression<Func<TController, TResult>> func)
            where TController : ControllerBase =>
            rootServiceProviderContainer.RootServiceProvider.GetDefaultControllerEvaluator<TController>().Evaluate(func);

        public void EvaluateController<TController>(Expression<Action<TController>> action)
            where TController : ControllerBase =>
            rootServiceProviderContainer.RootServiceProvider.GetDefaultControllerEvaluator<TController>().Evaluate(action);
    }
}
