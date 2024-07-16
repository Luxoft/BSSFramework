using Automation.Interfaces;

using Microsoft.Extensions.DependencyInjection;

using Xunit.Sdk;

namespace Automation.Xunit.Utils;

public static class TestInitializationUtil
{
    public static async Task InitializeTest(IServiceProvider testEnvServiceProvider, ExceptionAggregator aggregator)
    {
        if (testEnvServiceProvider is null)
        {
            return;
        }

        if (testEnvServiceProvider.GetService<ITestInitializeAndCleanup>() is {} initialization)
        {
            await aggregator.RunAsync(initialization.InitializeAsync);
        }
    }

    public static async Task CleanupTest(IServiceProvider testEnvServiceProvider, ExceptionAggregator aggregator)
    {
        if (testEnvServiceProvider is null)
        {
            return;
        }

        if (testEnvServiceProvider.GetService<ITestInitializeAndCleanup>() is {} initialization)
        {
            await aggregator.RunAsync(initialization.CleanupAsync);
        }
    }
}
