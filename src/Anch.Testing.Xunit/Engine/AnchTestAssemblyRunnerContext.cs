using System.Linq.Expressions;
using System.Reflection;

using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTestAssemblyRunnerContext(
    IXunitTestAssembly testAssembly,
    IReadOnlyCollection<IXunitTestCase> testCases,
    IMessageSink executionMessageSink,
    ITestFrameworkExecutionOptions executionOptions,
    CancellationToken cancellationToken,
    AnchTestCollectionRunner commonTestCollectionRunner)
    : XunitTestAssemblyRunnerContext(testAssembly, testCases, executionMessageSink, executionOptions, cancellationToken)
{
    private static readonly Func<XunitTestAssemblyRunnerBaseContext<IXunitTestAssembly, IXunitTestCase>, SemaphoreSlim?> GetParallelSemaphore = CreateSemaphoreSlimGetter();

    private static Func<XunitTestAssemblyRunnerBaseContext<IXunitTestAssembly, IXunitTestCase>, SemaphoreSlim?> CreateSemaphoreSlimGetter()
    {
        var instanceParam = Expression.Parameter(typeof(XunitTestAssemblyRunnerBaseContext<IXunitTestAssembly, IXunitTestCase>), "instance");

        var field = typeof(XunitTestAssemblyRunnerBaseContext<IXunitTestAssembly, IXunitTestCase>)
                        .GetField("parallelSemaphore", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?? throw new InvalidOperationException("Field not found");

        var fieldAccess = Expression.Field(instanceParam, field);

        return Expression.Lambda<Func<XunitTestAssemblyRunnerBaseContext<IXunitTestAssembly, IXunitTestCase>, SemaphoreSlim?>>(fieldAccess, instanceParam)
            .Compile();
    }

    public new async ValueTask<RunSummary> RunTestCollection(
        IXunitTestCollection testCollection,
        IReadOnlyCollection<IXunitTestCase> testCases,
        ITestCaseOrderer testCaseOrderer)
    {
        var parallelSemaphore = GetParallelSemaphore(this);

        if (parallelSemaphore is not null)
            await parallelSemaphore.WaitAsync(this.CancellationTokenSource.Token);

        try
        {
            return await commonTestCollectionRunner.Run(
                testCollection,
                testCases, this.ExplicitOption, this.MessageBus,
                testCaseOrderer, this.Aggregator.Clone(), this.CancellationTokenSource, this.AssemblyFixtureMappings);
        }
        finally
        {
            parallelSemaphore?.Release();
        }
    }
}