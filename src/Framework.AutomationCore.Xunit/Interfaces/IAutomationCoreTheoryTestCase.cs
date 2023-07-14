using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Interfaces;

public interface IAutomationCoreTheoryTestCase : IXunitTestCase
{
    Task<RunSummary> RunAsync(
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        object[] constructorArguments,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IServiceProvider testEnvServiceProvider);
}
