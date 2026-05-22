using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTestClassRunnerContext(
    IXunitTestClass testClass,
    IReadOnlyCollection<IXunitTestCase> testCases,
    ExplicitOption explicitOption,
    IMessageBus messageBus,
    ITestCaseOrderer testCaseOrderer,
    ExceptionAggregator aggregator,
    CancellationTokenSource cancellationTokenSource,
    FixtureMappingManager collectionFixtureMappings,
    IServiceProvider? serviceProvider)
    : XunitTestClassRunnerContext(testClass, testCases, explicitOption, messageBus, testCaseOrderer, aggregator, cancellationTokenSource,
        collectionFixtureMappings)
{
    public IServiceProvider? ServiceProvider { get; } = serviceProvider;
}