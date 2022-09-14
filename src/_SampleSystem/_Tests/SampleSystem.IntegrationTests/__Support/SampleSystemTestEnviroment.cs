using System;

using Automation;
using Automation.Utils.DatabaseUtils;
using Automation.Utils.DatabaseUtils.Interfaces;

using SampleSystem.IntegrationTests.__Support;
using SampleSystem.IntegrationTests.Support.Utils;

namespace SampleSystem.IntegrationTests;

public class SampleSystemTestEnvironment : TestEnvironment
{
    private SampleSystemTestEnvironment()
    {
    }

    protected override string EnvironmentPrefix => $"{nameof(SampleSystem)}_";

    protected override ServiceProviderPool BuildServiceProvidePool()
    {
        return new SampleSystemServiceProviderPool(this.RootConfiguration, this.ConfigUtil);
    }

    protected override TestDatabaseGenerator GetDatabaseGenerator(IServiceProvider serviceProvider, IDatabaseContext databaseContext)
    {
        return new SampleSystemTestDatabaseGenerator(databaseContext, this.ConfigUtil ,serviceProvider);
    }

    public static readonly SampleSystemTestEnvironment Current = new SampleSystemTestEnvironment();
}
