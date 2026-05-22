using Xunit.Internal;
using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTestMethodRunner : XunitTestMethodRunnerBase<AnchTestMethodRunnerContext, IXunitTestMethod, IXunitTestCase>
{
    public static AnchTestMethodRunner Instance { get; } = new();

    protected override async ValueTask<RunSummary> RunTestCase(AnchTestMethodRunnerContext ctxt, IXunitTestCase testCase)
    {
        Guard.ArgumentNotNull(ctxt);
        Guard.ArgumentNotNull(testCase);

        if (ctxt.ServiceProvider != null)
        {
            await ctxt.ServiceProvider.RunEnvironmentHooks(EnvironmentHookType.Before, ctxt.CancellationTokenSource.Token);
        }

        try
        {
            if (testCase is ISelfExecutingXunitTestCase selfExecutingTestCase)
                return await selfExecutingTestCase.Run(ctxt.ExplicitOption, ctxt.MessageBus, ctxt.ConstructorArguments, ctxt.Aggregator.Clone(), ctxt.CancellationTokenSource);

            return await AnchRunnerHelper.RunXunitTestCase(
                testCase,
                ctxt.MessageBus,
                ctxt.CancellationTokenSource,
                ctxt.Aggregator.Clone(),
                ctxt.ExplicitOption,
                ctxt.ConstructorArguments
            );
        }
        finally
        {
            if (ctxt.ServiceProvider != null)
            {
                await ctxt.ServiceProvider.RunEnvironmentHooks(EnvironmentHookType.After, ctxt.CancellationTokenSource.Token);
            }
        }
    }

    public async ValueTask<RunSummary> Run(
        IXunitTestMethod testMethod,
        IReadOnlyCollection<IXunitTestCase> testCases,
        ExplicitOption explicitOption,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        object?[] constructorArguments,
        IServiceProvider? serviceProvider)
    {
        Guard.ArgumentNotNull(testCases);
        Guard.ArgumentNotNull(messageBus);
        Guard.ArgumentNotNull(constructorArguments);

        await using var ctxt = new AnchTestMethodRunnerContext(
            testMethod,
            testCases,
            explicitOption,
            messageBus,
            aggregator,
            cancellationTokenSource,
            constructorArguments,
            serviceProvider
        );

        await ctxt.InitializeAsync();

        return await this.Run(ctxt);
    }
}