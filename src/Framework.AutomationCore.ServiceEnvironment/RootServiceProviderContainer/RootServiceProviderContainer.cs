using System;

using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.DomainDriven;
using Microsoft.AspNetCore.Mvc;

namespace Automation.ServiceEnvironment;

public abstract class RootServiceProviderContainer : IRootServiceProviderContainer
{
    public RootServiceProviderContainer(IServiceProvider rootServiceProvider)
    {
        this.RootServiceProvider = rootServiceProvider;
    }

    public virtual IServiceProvider RootServiceProvider { get; }

    public ConfigUtil ConfigUtil => this.GetConfigUtil();

    public IDateTimeService DateTimeService => this.GetDateTimeService();

    public IDatabaseContext DatabaseContext => this.GetDatabaseContext();

    public virtual ControllerEvaluator<TController> GetControllerEvaluator<TController>(string principalName = null)
            where TController : ControllerBase
    {
        return this.RootServiceProvider.GetDefaultControllerEvaluator<TController>(principalName);
    }
}

public abstract class RootServiceProviderContainer<TBLLContext> : RootServiceProviderContainer, IRootServiceProviderContainer<TBLLContext>
{
    protected RootServiceProviderContainer(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }
}
