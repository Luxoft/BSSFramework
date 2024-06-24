using System.Reflection;

using Automation.Xunit.Interfaces;
using Automation.Xunit.Runners;
using Automation.Xunit.Utils;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit;

public class AutomationCoreFrameworkExecutor : XunitTestFrameworkExecutor
{
    internal static IServiceProvider FwServiceProvider { get; private set; }

    protected ExceptionAggregator Aggregator { get; set; } = new ExceptionAggregator();

    public AutomationCoreFrameworkExecutor(
        AssemblyName assemblyName,
        ISourceInformationProvider sourceInformationProvider,
        IMessageSink diagnosticMessageSink)
        : base(assemblyName, sourceInformationProvider, diagnosticMessageSink)
    {
        try
        {
            var serviceProviderBuilder = FrameworkInitializationUtil.TryGetImplementationInstanceOf<IAutomationCoreServiceProviderBuilder>(assemblyName)
                                         ?? new AutomationCoreServiceProviderBuilder();

            FwServiceProvider = serviceProviderBuilder.GetFrameworkServiceProvider(assemblyName);
        }
        catch (Exception e)
        {
            this.DiagnosticMessageSink.OnMessage(new DiagnosticMessage($"Service Provider Initialization error: {e.Message}"));
            this.Aggregator.Add(e);
        }
    }

    protected override async void RunTestCases(
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions)
    {
        using var assemblyRunner = new AutomationCoreAssemblyRunner(
            FwServiceProvider,
            this.TestAssembly,
            testCases,
            this.DiagnosticMessageSink,
            executionMessageSink,
            executionOptions,
            this.Aggregator);

        await assemblyRunner.RunAsync();
    }
}
