using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.FinancialYear;
using Framework.SecuritySystem.Credential;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public abstract class RootServiceProviderContainer(IServiceProvider rootServiceProvider) : IRootServiceProviderContainer
{
    public virtual IServiceProvider RootServiceProvider { get; } = rootServiceProvider;

    public AutomationFrameworkSettings AutomationFrameworkSettings => this.GetAutomationFrameworkSettings();

    public TimeProvider TimeProvider => this.GetTimeProvider();

    public IFinancialYearService FinancialYearService => this.RootServiceProvider.GetRequiredService<IFinancialYearService>();

    public IDatabaseContext DatabaseContext => this.GetDatabaseContext();

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(UserCredential? userCredential = null)
            where TController : ControllerBase
    {
        return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(userCredential);
    }
}
