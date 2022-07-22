using System;

using SampleSystem.IntegrationTests;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.BLL.Test;

public abstract class TestBase : IRootServiceProviderContainer
{
    public IServiceProvider RootServiceProvider { get; } = SampleSystemTestRootServiceProvider.Default;
}
