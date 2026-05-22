using System.Globalization;

using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchFrameworkExecutor(IXunitTestAssembly testAssembly, AnchTestAssemblyRunner commonXunitTestAssemblyRunner, IServiceProviderPool? serviceProviderPool) : XunitTestFrameworkExecutor(testAssembly)
{
    public override async ValueTask RunTestCases(IReadOnlyCollection<IXunitTestCase> testCases, IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions,
        CancellationToken cancellationToken)
    {
        SetEnvironment(EnvironmentVariables.AssertEquivalentMaxDepth, executionOptions.AssertEquivalentMaxDepth());
        SetEnvironment(EnvironmentVariables.PrintMaxEnumerableLength, executionOptions.PrintMaxEnumerableLength());
        SetEnvironment(EnvironmentVariables.PrintMaxObjectDepth, executionOptions.PrintMaxObjectDepth());
        SetEnvironment(EnvironmentVariables.PrintMaxObjectMemberCount, executionOptions.PrintMaxObjectMemberCount());
        SetEnvironment(EnvironmentVariables.PrintMaxStringLength, executionOptions.PrintMaxStringLength());

        try
        {
            await commonXunitTestAssemblyRunner.Run(this.TestAssembly, testCases, executionMessageSink, executionOptions, cancellationToken);
        }
        finally
        {
            if (serviceProviderPool != null)
            {
                await serviceProviderPool.DisposeAsync();
            }
        }
    }

    private static void SetEnvironment(
        string environmentVariableName,
        int? value)
    {
        if (value.HasValue)
            Environment.SetEnvironmentVariable(environmentVariableName, value.Value.ToString(CultureInfo.InvariantCulture));
    }
}