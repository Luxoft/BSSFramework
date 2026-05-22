using Xunit.Internal;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTestCaseRunner : XunitTestCaseRunner
{
    public new static AnchTestCaseRunner Instance { get; } = new ();

    protected override ValueTask<RunSummary> RunTest(XunitTestCaseRunnerContext ctxt, IXunitTest test)
    {
        Guard.ArgumentNotNull(ctxt);
        Guard.ArgumentNotNull(test);

        return AnchTestRunner.Instance.Run(
            test,
            ctxt.MessageBus,
            ctxt.ConstructorArguments,
            ctxt.ExplicitOption,
            ctxt.Aggregator.Clone(),
            ctxt.CancellationTokenSource,
            ctxt.BeforeAfterTestAttributes
        );
    }
}