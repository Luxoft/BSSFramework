using System.Reflection;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

public class AutomationCoreTestCollectionRunner : XunitTestCollectionRunner
{
    private readonly IServiceProvider fwServiceProvider;

    public AutomationCoreTestCollectionRunner(
        IServiceProvider fwServiceProvider,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ITestCaseOrderer testCaseOrderer,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource)
        : base(testCollection, testCases, diagnosticMessageSink, messageBus, testCaseOrderer, aggregator, cancellationTokenSource) =>
        this.fwServiceProvider = fwServiceProvider;

    protected override async Task AfterTestCollectionStartingAsync()
    {
        await this.CreateCollectionFixturesAsync();
        this.TestCaseOrderer = this.GetTestCaseOrderer() ?? this.TestCaseOrderer;
    }

    /// <inheritdoc/>
    protected override async Task BeforeTestCollectionFinishedAsync()
    {
        var disposeAsyncTasks = this.CollectionFixtureMappings.Values.OfType<IAsyncLifetime>().Select(fixture => this.Aggregator.RunAsync(fixture.DisposeAsync)).ToList();

        await Task.WhenAll(disposeAsyncTasks);

        foreach (var fixture in this.CollectionFixtureMappings.Values.OfType<IDisposable>())
            this.Aggregator.Run(fixture.Dispose);
    }

    async Task CreateCollectionFixturesAsync()
    {
        if (this.TestCollection.CollectionDefinition is not null)
        {
            var declarationType = ((IReflectionTypeInfo)this.TestCollection.CollectionDefinition).Type;
            foreach (var interfaceType in declarationType.GetTypeInfo().ImplementedInterfaces.Where(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollectionFixture<>)))
            {
                var fixtureType = interfaceType.GenericTypeArguments.Single();
                this.CreateCollectionFixture(fixtureType);
            }

            var initializeAsyncTasks = this.CollectionFixtureMappings.Values.OfType<IAsyncLifetime>().Select(fixture => this.Aggregator.RunAsync(fixture.InitializeAsync)).ToList();
            await Task.WhenAll(initializeAsyncTasks);
        }
    }

    protected override Task<RunSummary> RunTestClassAsync(
        ITestClass testClass,
        IReflectionTypeInfo @class,
        IEnumerable<IXunitTestCase> testCases)
        => new AutomationCoreTestClassRunner(
            this.fwServiceProvider,
            testClass,
            @class,
            testCases,
            this.DiagnosticMessageSink,
            this.MessageBus,
            this.TestCaseOrderer,
            new ExceptionAggregator(this.Aggregator),
            this.CancellationTokenSource,
            this.CollectionFixtureMappings).RunAsync();
}
