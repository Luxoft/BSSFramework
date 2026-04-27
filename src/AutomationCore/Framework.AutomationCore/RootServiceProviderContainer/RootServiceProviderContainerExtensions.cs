using System.Linq.Expressions;

using Anch.SecuritySystem;

using Framework.Application.FinancialYear;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.ServiceEnvironment.Services;
using Framework.AutomationCore.Settings;
//using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public static class RootServiceProviderContainerExtensions
{
    extension(IRootServiceProviderContainer rootServiceProviderContainer)
    {
        //public IDatabaseContext GetDatabaseContext() => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IDatabaseContext>();



        public AutomationFrameworkSettings AutomationFrameworkSettings => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IOptions<AutomationFrameworkSettings>>().Value;

        public TimeProvider TimeProvider => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<TimeProvider>();

        public IFinancialYearService FinancialYearService => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IFinancialYearService>();

        //public IDatabaseContext DatabaseContext => this.GetDatabaseContext();


        public void SetCurrentDateTime(DateTime newDateTime) => rootServiceProviderContainer.RootServiceProvider.GetRequiredService<IntegrationTestTimeProvider>().SetCurrentDateTime(newDateTime);

        public TResult EvaluateController<TController, TResult>(Expression<Func<TController, TResult>> func)
            where TController : ControllerBase =>

            rootServiceProviderContainer.GetControllerEvaluator<TController>().Evaluate(func);

        public void EvaluateController<TController>(Expression<Action<TController>> action)
            where TController : ControllerBase =>
            rootServiceProviderContainer.GetControllerEvaluator<TController>().Evaluate(action);
    }
}
