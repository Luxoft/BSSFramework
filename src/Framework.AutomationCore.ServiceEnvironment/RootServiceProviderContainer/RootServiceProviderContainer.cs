using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.FinancialYear;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.ServiceEnvironment;

public abstract class RootServiceProviderContainer : IRootServiceProviderContainer
{
    public RootServiceProviderContainer(IServiceProvider rootServiceProvider)
    {
        this.RootServiceProvider = rootServiceProvider;
    }

    public virtual IServiceProvider RootServiceProvider { get; }

    public ConfigUtil ConfigUtil => this.GetConfigUtil();

    public TimeProvider TimeProvider => this.GetTimeProvider();

    public IFinancialYearService FinancialYearService => this.RootServiceProvider.GetRequiredService<IFinancialYearService>();

    public IDatabaseContext DatabaseContext => this.GetDatabaseContext();

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase
    {
        return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }
}
