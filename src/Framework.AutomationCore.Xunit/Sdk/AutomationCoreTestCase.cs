using System.ComponentModel;

using Automation.Xunit.Interfaces;
using Automation.Xunit.Runners;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Sdk;

public class AutomationCoreTestCase : XunitTestCase, IAutomationCoreTheoryTestCase
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public AutomationCoreTestCase()
    {
    }

    public AutomationCoreTestCase(
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod,
        object[] testMethodArguments = null)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod, testMethodArguments)
    {
    }

    public Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IServiceProvider testEnvServiceProvider)
        => new AutomationCoreTestCaseRunner(
            this,
            this.DisplayName,
            this.SkipReason,
            constructorArguments,
            this.TestMethodArguments,
            messageBus,
            aggregator,
            cancellationTokenSource,
            testEnvServiceProvider).RunAsync();
}
