using System.Linq.Expressions;

using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;
using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment.Services;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.ServiceEnvironment.RootServiceProviderContainer;

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
