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
            aggregator.Run(initialization.Initialize);
        }

        if (testEnvServiceProvider.GetService<ITestInitializeAndCleanupAsync>() is {} initializationAsync)
        {
            await aggregator.RunAsync(initializationAsync.InitializeAsync);
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
            aggregator.Run(initialization.Cleanup);
        }

        if (testEnvServiceProvider.GetService<ITestInitializeAndCleanupAsync>() is {} initializationAsync)
        {
            await aggregator.RunAsync(initializationAsync.CleanupAsync);
        }
    }
}
