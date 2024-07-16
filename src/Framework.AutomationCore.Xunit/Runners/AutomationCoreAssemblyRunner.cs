using System.Security;

using Automation.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

/// <summary>
/// Forked implementation of the test assembly runner for xUnit.net v2 tests.
/// Parallelization is controlled by config
/// </summary>

public class AutomationCoreAssemblyRunner : TestAssemblyRunner<IXunitTestCase>
{
    bool disableParallelization;

    bool initialized;

    int maxParallelThreads;

    SynchronizationContext originalSyncContext;

    MaxConcurrencySyncContext syncContext;

    private readonly IServiceProvider fwServiceProvider;

    public AutomationCoreAssemblyRunner(
        IServiceProvider fwServiceProvider,
        ITestAssembly testAssembly,
        IEnumerable<IXunitTestCase> testCases,
        IMessageSink diagnosticMessageSink,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions,
        ExceptionAggregator aggregator)
        : base(testAssembly, testCases, diagnosticMessageSink, executionMessageSink, executionOptions)
    {
        this.fwServiceProvider = fwServiceProvider;
        this.Aggregator = aggregator;
    }

    public override void Dispose()
    {
        if (this.syncContext is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }

    protected override string GetTestFrameworkDisplayName()
        => XunitTestFrameworkDiscoverer.DisplayName;

    protected override string GetTestFrameworkEnvironment()
    {
        this.Initialize();

        var testCollectionFactory = ExtensibilityPointFactory.GetXunitTestCollectionFactory(
            this.DiagnosticMessageSink,
            (IAttributeInfo)null,
            this.TestAssembly);

        var threadCountText = this.maxParallelThreads < 0 ? "unlimited" : this.maxParallelThreads.ToString();

        return
            $"{base.GetTestFrameworkEnvironment()} [{testCollectionFactory.DisplayName}, {(this.disableParallelization ? "non-parallel" : $"parallel ({threadCountText} threads)")}]";
    }

    protected virtual void SetupSyncContext(int maxThreads)
    {
        if (MaxConcurrencySyncContext.IsSupported && maxThreads > 0)
        {
            this.syncContext = new MaxConcurrencySyncContext(maxThreads);
            SetSynchronizationContext(this.syncContext);
        }
    }

    protected void Initialize()
    {
        if (this.initialized)
        {
            return;
        }

        if (this.fwServiceProvider is not null)
        {
            var configuration = this.fwServiceProvider.GetRequiredService<IConfiguration>();
            this.disableParallelization = !configuration.GetValue<bool>("TestsParallelize");
            this.maxParallelThreads = configuration.GetValue<int>("MaxParallelThreads");
        }

        this.disableParallelization = this.ExecutionOptions.DisableParallelization() ?? this.disableParallelization;
        this.maxParallelThreads = this.ExecutionOptions.MaxParallelThreads() ?? this.maxParallelThreads;

        if (this.maxParallelThreads == 0)
        {
            this.maxParallelThreads = Environment.ProcessorCount;
        }

        var testCaseOrdererAttribute = this.TestAssembly.Assembly.GetCustomAttributes(typeof(TestCaseOrdererAttribute)).SingleOrDefault();
        if (testCaseOrdererAttribute != null)
        {
            try
            {
                var testCaseOrderer = ExtensibilityPointFactory.GetTestCaseOrderer(this.DiagnosticMessageSink, testCaseOrdererAttribute);
                if (testCaseOrderer != null)
                    this.TestCaseOrderer = testCaseOrderer;
                else
                {
                    var args = testCaseOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    this.DiagnosticMessageSink.OnMessage(
                        new DiagnosticMessage($"Could not find type '{args[0]}' in {args[1]} for assembly-level test case orderer"));
                }
            }
            catch (Exception ex)
            {
                var args = testCaseOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                this.DiagnosticMessageSink.OnMessage(
                    new DiagnosticMessage(
                        $"Assembly-level test case orderer '{args[0]}' threw '{ex.GetType().FullName}' during construction: {ex.Message}{Environment.NewLine}{ex.StackTrace}"));
            }
        }

        var testCollectionOrdererAttribute =
            this.TestAssembly.Assembly.GetCustomAttributes(typeof(TestCollectionOrdererAttribute)).SingleOrDefault();
        if (testCollectionOrdererAttribute != null)
        {
            try
            {
                var testCollectionOrderer = ExtensibilityPointFactory.GetTestCollectionOrderer(
                    this.DiagnosticMessageSink,
                    testCollectionOrdererAttribute);
                if (testCollectionOrderer != null)
                    this.TestCollectionOrderer = testCollectionOrderer;
                else
                {
                    var args = testCollectionOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    this.DiagnosticMessageSink.OnMessage(
                        new DiagnosticMessage($"Could not find type '{args[0]}' in {args[1]} for assembly-level test collection orderer"));
                }
            }
            catch (Exception ex)
            {
                var args = testCollectionOrdererAttribute.GetConstructorArguments().Cast<string>().ToList();
                this.DiagnosticMessageSink.OnMessage(
                    new DiagnosticMessage(
                        $"Assembly-level test collection orderer '{args[0]}' threw '{ex.GetType().FullName}' during construction: {ex.Message}{Environment.NewLine}{ex.StackTrace}"));
            }
        }

        this.initialized = true;
    }

    protected override async Task AfterTestAssemblyStartingAsync() =>
        await this.Aggregator.RunAsync(
            async () =>
            {
                this.Initialize();
                await this.ExecuteInitializationAsync();
            });

    protected override async Task BeforeTestAssemblyFinishedAsync() =>
        await this.Aggregator.RunAsync(
            async () =>
            {
                SetSynchronizationContext(this.originalSyncContext);
                await this.ExecuteCleanupAsync();
            });

    protected override async Task<RunSummary> RunTestCollectionsAsync(
        IMessageBus messageBus,
        CancellationTokenSource cancellationTokenSource)
    {
        this.originalSyncContext = SynchronizationContext.Current;

        if (this.disableParallelization)
        {
            return await base.RunTestCollectionsAsync(messageBus, cancellationTokenSource);
        }

        this.SetupSyncContext(this.maxParallelThreads);

        Func<Func<Task<RunSummary>>, Task<RunSummary>> taskRunner;
        if (SynchronizationContext.Current != null)
        {
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            taskRunner = code => Task.Factory.StartNew(
                             code,
                             cancellationTokenSource.Token,
                             TaskCreationOptions.DenyChildAttach | TaskCreationOptions.HideScheduler,
                             scheduler).Unwrap();
        }
        else
        {
            taskRunner = code => Task.Run(code, cancellationTokenSource.Token);
        }

        List<Task<RunSummary>> parallel = null;
        List<Func<Task<RunSummary>>> nonParallel = null;
        var summaries = new List<RunSummary>();

        foreach (var collection in this.OrderTestCollections())
        {
            var task = () => this.RunTestCollectionAsync(
                           messageBus,
                           collection.Item1,
                           collection.Item2,
                           cancellationTokenSource);

            // attr is null here from our new unit test, but I'm not sure if that's expected or there's a cheaper approach here
            // Current approach is trying to avoid any changes to the abstractions at all
            var attr = collection.Item1.CollectionDefinition?.GetCustomAttributes(typeof(CollectionDefinitionAttribute)).SingleOrDefault();
            if (attr?.GetNamedArgument<bool>(nameof(CollectionDefinitionAttribute.DisableParallelization)) == true)
            {
                (nonParallel ??= new List<Func<Task<RunSummary>>>()).Add(task);
            }
            else
            {
                (parallel ??= new List<Task<RunSummary>>()).Add(taskRunner(task));
            }
        }

        if (parallel?.Count > 0)
        {
            foreach (var task in parallel)
            {
                try
                {
                    summaries.Add(await task);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        if (nonParallel?.Count > 0)
        {
            foreach (var task in nonParallel)
            {
                try
                {
                    summaries.Add(await taskRunner(task));
                    if (cancellationTokenSource.IsCancellationRequested)
                        break;
                }
                catch (TaskCanceledException)
                {
                }
            }
        }

        return new RunSummary()
               {
                   Total = summaries.Sum(s => s.Total), Failed = summaries.Sum(s => s.Failed), Skipped = summaries.Sum(s => s.Skipped)
               };
    }

    protected async Task ExecuteInitializationAsync()
    {
        if (this.fwServiceProvider is null)
        {
            return;
        }

        if (this.fwServiceProvider.GetService<IAssemblyInitializeAndCleanup>() is { } initialization)
        {
            await this.Aggregator.RunAsync(initialization.EnvironmentInitializeAsync);
        }
    }

    protected async Task ExecuteCleanupAsync()
    {
        if (this.fwServiceProvider is null)
        {
            return;
        }

        if (this.fwServiceProvider.GetService<IAssemblyInitializeAndCleanup>() is { } initialization)
        {
            await this.Aggregator.RunAsync(initialization.EnvironmentCleanupAsync);
        }
    }

    protected override async Task<RunSummary> RunTestCollectionAsync(
        IMessageBus messageBus,
        ITestCollection testCollection,
        IEnumerable<IXunitTestCase> testCases,
        CancellationTokenSource cancellationTokenSource) =>
        await new AutomationCoreTestCollectionRunner(
            this.fwServiceProvider,
            testCollection,
            testCases,
            this.DiagnosticMessageSink,
            messageBus,
            this.TestCaseOrderer,
            new ExceptionAggregator(this.Aggregator),
            cancellationTokenSource).RunAsync();

    [SecuritySafeCritical]
    static void SetSynchronizationContext(SynchronizationContext context)
        => SynchronizationContext.SetSynchronizationContext(context);
}
