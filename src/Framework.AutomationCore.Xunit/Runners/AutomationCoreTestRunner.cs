using System.Reflection;

using Automation.Xunit.Utils;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

public class AutomationCoreTestRunner : XunitTestRunner
{
    private readonly IServiceProvider testEnvServiceProvider;

    public AutomationCoreTestRunner(
        ITest test,
        IMessageBus messageBus,
        Type testClass,
        object[] constructorArguments,
        MethodInfo testMethod,
        object[] testMethodArguments,
        string skipReason,
        IReadOnlyList<BeforeAfterTestAttribute> beforeAfterAttributes,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IServiceProvider testEnvServiceProvider)
        : base(
            test,
            messageBus,
            testClass,
            constructorArguments,
            testMethod,
            testMethodArguments,
            skipReason,
            beforeAfterAttributes,
            aggregator,
            cancellationTokenSource) =>
        this.testEnvServiceProvider = testEnvServiceProvider;

    protected override async Task<Tuple<decimal, string>> InvokeTestAsync(ExceptionAggregator aggregator)
    {
        await TestInitializationUtil.InitializeTest(this.testEnvServiceProvider, this.Aggregator);

        var result = await base.InvokeTestAsync(aggregator);

        await TestInitializationUtil.CleanupTest(this.testEnvServiceProvider, this.Aggregator);

        return result;
    }
}
