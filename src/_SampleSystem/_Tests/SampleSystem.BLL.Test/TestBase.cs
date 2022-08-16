using System;
using Automation.ServiceEnvironment;
using SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

namespace SampleSystem.BLL.Test;

public abstract class TestBase : IRootServiceProviderContainer
{
    private IServiceProvider rootServiceProvider;

    public IServiceProvider RootServiceProvider => rootServiceProvider ??= SampleSystemTestRootServiceProvider.Create();
}
