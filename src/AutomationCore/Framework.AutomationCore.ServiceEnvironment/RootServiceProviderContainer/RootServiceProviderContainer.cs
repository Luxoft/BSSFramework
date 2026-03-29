using Framework.Application.FinancialYear;
using Framework.AutomationCore.ServiceEnvironment.ServiceEnvironment;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Credential;

namespace Framework.AutomationCore.ServiceEnvironment.RootServiceProviderContainer;

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
