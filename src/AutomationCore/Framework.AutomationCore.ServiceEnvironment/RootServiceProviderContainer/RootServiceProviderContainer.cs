using Framework.Application.FinancialYear;
using Framework.AutomationCore.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;

namespace Framework.AutomationCore.RootServiceProviderContainer;

public abstract class RootServiceProviderContainer(IServiceProvider rootServiceProvider) : IRootServiceProviderContainer
{
    public virtual IServiceProvider RootServiceProvider { get; } = rootServiceProvider;

    public AutomationFrameworkSettings AutomationFrameworkSettings => this.GetAutomationFrameworkSettings();

    public TimeProvider TimeProvider => this.GetTimeProvider();

    public IFinancialYearService FinancialYearService => this.RootServiceProvider.GetRequiredService<IFinancialYearService>();

    public IDatabaseContext DatabaseContext => this.GetDatabaseContext();

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(UserCredential? userCredential = null)
            where TController : ControllerBase =>
        this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(userCredential);
}
