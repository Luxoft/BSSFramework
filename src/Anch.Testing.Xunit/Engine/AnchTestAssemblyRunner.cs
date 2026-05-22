using Xunit.Internal;
using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTestAssemblyRunner(AnchTestCollectionRunner commonTestCollectionRunner) : XunitTestAssemblyRunnerBase<AnchTestAssemblyRunnerContext, IXunitTestAssembly, IXunitTestCollection, IXunitTestCase>
{
    public async ValueTask<RunSummary> Run(
        IXunitTestAssembly testAssembly,
        IReadOnlyCollection<IXunitTestCase> testCases,
        IMessageSink executionMessageSink,
        ITestFrameworkExecutionOptions executionOptions,
        CancellationToken cancellationToken)
    {
        Guard.ArgumentNotNull(testAssembly);
        Guard.ArgumentNotNull(testCases);
        Guard.ArgumentNotNull(executionMessageSink);
        Guard.ArgumentNotNull(executionOptions);

        await using var ctxt = new AnchTestAssemblyRunnerContext(
            testAssembly,
            testCases,
            executionMessageSink,
            executionOptions,
            cancellationToken, commonTestCollectionRunner
        );

        await ctxt.InitializeAsync();

        return await this.Run(ctxt);
    }

    protected override ValueTask<RunSummary> RunTestCollection(AnchTestAssemblyRunnerContext ctxt, IXunitTestCollection testCollection, IReadOnlyCollection<IXunitTestCase> testCases)
    {
        Guard.ArgumentNotNull(ctxt);
        Guard.ArgumentNotNull(testCollection);
        Guard.ArgumentNotNull(testCases);

        var testCaseOrderer = ctxt.AssemblyTestCaseOrderer ?? DefaultTestCaseOrderer.Instance;

        return ctxt.RunTestCollection(testCollection, testCases, testCaseOrderer);
    }
}