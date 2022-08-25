using System;
using Automation.Utils;
using Automation.Utils.DatabaseUtils.Interfaces;
using Microsoft.Extensions.Configuration;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.IntegrationTests.__Support;

public class SampleSystemServiceProviderPool : ServiceProviderPool
{
    public SampleSystemServiceProviderPool(IConfiguration rootRootConfiguration, ConfigUtil configUtil) : base(rootRootConfiguration, configUtil)
    {
    }

    protected override IServiceProvider Build(IDatabaseContext databaseContext)
    {
        return SampleSystemTestRootServiceProvider.Create(this.RootConfiguration, databaseContext, this.ConfigUtil);
    }
}