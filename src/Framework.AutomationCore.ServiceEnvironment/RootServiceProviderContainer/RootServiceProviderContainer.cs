using System;

using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;

using Framework.DomainDriven;

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
}

public abstract class RootServiceProviderContainer<TBLLContext> : RootServiceProviderContainer, IRootServiceProviderContainer<TBLLContext>
{
    protected RootServiceProviderContainer(IServiceProvider rootServiceProvider)
            : base(rootServiceProvider)
    {
    }
}
