using System.Reflection;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

public class AutomationCoreTestCaseRunner : XunitTestCaseRunner
{
    protected readonly IServiceProvider TestEnvServiceProvider;

    public AutomationCoreTestCaseRunner(
        IXunitTestCase testCase,
        string displayName,
        string skipReason,
        object[] constructorArguments,
        object[] testMethodArguments,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IServiceProvider testEnvServiceProvider)
        : base(
            testCase,
            displayName,
            skipReason,
            constructorArguments,
            testMethodArguments,
            messageBus,
            aggregator,
            cancellationTokenSource) =>
        this.TestEnvServiceProvider = testEnvServiceProvider;

    protected override XunitTestRunner CreateTestRunner(
        ITest test,
        IMessageBus messageBus,
        Type testClass,
        object[] constructorArguments,
        MethodInfo testMethod,
        object[] testMethodArguments,
        string skipReason,
        IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
        => new AutomationCoreTestRunner(
            test,
            messageBus,
            testClass,
            constructorArguments,
            testMethod,
            testMethodArguments,
            skipReason,
            beforeAfterAttributes,
            new ExceptionAggregator(aggregator),
            cancellationTokenSource,
            this.TestEnvServiceProvider);
}
