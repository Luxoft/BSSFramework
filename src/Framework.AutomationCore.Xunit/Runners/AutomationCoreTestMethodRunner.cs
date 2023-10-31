using Automation.Xunit.Interfaces;
using Automation.Xunit.Utils;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

public class AutomationCoreTestMethodRunner : TestMethodRunner<IXunitTestCase>
{
    readonly object[] constructorArguments;

    readonly IMessageSink diagnosticMessageSink;

    private readonly IServiceProvider testEnvServiceProvider;

    public AutomationCoreTestMethodRunner(
        ITestMethod testMethod,
        IReflectionTypeInfo @class,
        IReflectionMethodInfo method,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        object[] constructorArguments,
        IServiceProvider testEnvServiceProvider)
        : base(testMethod, @class, method, testCases, messageBus, aggregator, cancellationTokenSource)
    {
        this.constructorArguments = constructorArguments;
        this.diagnosticMessageSink = diagnosticMessageSink;
        this.testEnvServiceProvider = testEnvServiceProvider;
    }

    protected override Task<RunSummary> RunTestCaseAsync(IXunitTestCase testCase)
    {
        if (testCase.GetType().GetInterface(nameof(IAutomationCoreTheoryTestCase)) != null)
        {
            return ((IAutomationCoreTheoryTestCase)testCase).RunAsync(
                this.diagnosticMessageSink,
                this.MessageBus,
                this.constructorArguments,
                new ExceptionAggregator(this.Aggregator),
                this.CancellationTokenSource,
                this.testEnvServiceProvider);
        }

        return testCase.RunAsync(
            this.diagnosticMessageSink,
            this.MessageBus,
            this.constructorArguments,
            new ExceptionAggregator(this.Aggregator),
            this.CancellationTokenSource);
    }

    protected override async void AfterTestMethodStarting()
    {
        try
        {
            await TestInitializationUtil.InitializeTest(this.testEnvServiceProvider, this.Aggregator);
        }
        catch (Exception e)
        {
            this.Aggregator.Add(e);
        }

        base.AfterTestMethodStarting();
    }

    protected override async void BeforeTestMethodFinished()
    {
        base.BeforeTestMethodFinished();

        try
        {
            await TestInitializationUtil.CleanupTest(this.testEnvServiceProvider, this.Aggregator);
        }
        catch (Exception e)
        {
            this.Aggregator.Add(e);
        }
    }
}
