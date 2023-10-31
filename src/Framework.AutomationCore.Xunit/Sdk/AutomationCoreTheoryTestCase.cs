using System.ComponentModel;

using Automation.Xunit.Interfaces;
using Automation.Xunit.Runners;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Sdk;

public class AutomationCoreTheoryTestCase : XunitTheoryTestCase, IAutomationCoreTheoryTestCase
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete("Called by the de-serializer; should only be called by deriving classes for de-serialization purposes")]
    public AutomationCoreTheoryTestCase() {}

    public AutomationCoreTheoryTestCase(
        IMessageSink diagnosticMessageSink,
        TestMethodDisplay defaultMethodDisplay,
        TestMethodDisplayOptions defaultMethodDisplayOptions,
        ITestMethod testMethod)
        : base(diagnosticMessageSink, defaultMethodDisplay, defaultMethodDisplayOptions, testMethod)
    {
    }

    public Task<RunSummary> RunAsync(IMessageSink diagnosticMessageSink,
                                              IMessageBus messageBus,
                                              object[] constructorArguments,
                                              ExceptionAggregator aggregator,
                                              CancellationTokenSource cancellationTokenSource,
                                              IServiceProvider testEnvServiceProvider)
        => new AutomationCoreTheoryTestCaseRunner(
            this,
            this.DisplayName,
            this.SkipReason,
            constructorArguments,
            diagnosticMessageSink,
            messageBus,
            aggregator,
            cancellationTokenSource,
            testEnvServiceProvider).RunAsync();
}
