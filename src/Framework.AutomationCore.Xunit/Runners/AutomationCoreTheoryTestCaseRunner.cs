using System.Globalization;
using System.Reflection;
using Automation.Xunit.Sdk;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Runners;

public class AutomationCoreTheoryTestCaseRunner : XunitTestCaseRunner
{
    private readonly IServiceProvider testEnvServiceProvider;

    static readonly object[] NoArguments = Array.Empty<object>();

    readonly List<IDisposable> toDispose = new List<IDisposable>();

    Exception dataDiscoveryException;

    readonly List<XunitTestRunner> testRunners = new List<XunitTestRunner>();

    readonly ExceptionAggregator cleanupAggregator = new ExceptionAggregator();

    protected IMessageSink DiagnosticMessageSink { get; }

    public AutomationCoreTheoryTestCaseRunner(
        IXunitTestCase testCase,
        string displayName,
        string skipReason,
        object[] constructorArguments,
        IMessageSink diagnosticMessageSink,
        IMessageBus messageBus,
        ExceptionAggregator aggregator,
        CancellationTokenSource cancellationTokenSource,
        IServiceProvider testEnvServiceProvider)
        : base(testCase, displayName, skipReason, constructorArguments, NoArguments, messageBus, aggregator, cancellationTokenSource)
    {
        this.DiagnosticMessageSink = diagnosticMessageSink;
        this.testEnvServiceProvider = testEnvServiceProvider;
    }

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
        CancellationTokenSource cancellationTokenSource) =>
        new AutomationCoreTestRunner(
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
            this.testEnvServiceProvider);

    protected override async Task AfterTestCaseStartingAsync()
        {
            await base.AfterTestCaseStartingAsync();

            try
            {
                var dataAttributes = this.TestCase.TestMethod.Method.GetCustomAttributes(typeof(DataAttribute));

                foreach (var dataAttribute in dataAttributes)
                {
                    var discovererAttribute = dataAttribute.GetCustomAttributes(typeof(DataDiscovererAttribute)).First();
                    var args = discovererAttribute.GetConstructorArguments().Cast<string>().ToList();
                    var discovererType = SerializationHelper.GetType(args[1], args[0]);
                    if (discovererType == null)
                    {
                        if (dataAttribute is IReflectionAttributeInfo reflectionAttribute)
                        {
                            this.Aggregator.Add(
                                new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        "Data discoverer specified for {0} on {1}.{2} does not exist.",
                                        reflectionAttribute.Attribute.GetType(),
                                        this.TestCase.TestMethod.TestClass.Class.Name,
                                        this.TestCase.TestMethod.Method.Name
                                        )
                                    )
                                );
                        }
                        else
                        {
                            this.Aggregator.Add(
                                new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        "A data discoverer specified on {0}.{1} does not exist.",
                                        this.TestCase.TestMethod.TestClass.Class.Name,
                                        this.TestCase.TestMethod.Method.Name
                                        )
                                    )
                                );
                        }

                        continue;
                    }

                    IDataDiscoverer discoverer;
                    try
                    {
                        discoverer = ExtensibilityPointFactory.GetDataDiscoverer(this.DiagnosticMessageSink, discovererType);
                    }
                    catch (InvalidCastException)
                    {
                        if (dataAttribute is IReflectionAttributeInfo reflectionAttribute)
                        {
                            this.Aggregator.Add(
                                new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        "Data discoverer specified for {0} on {1}.{2} does not implement IDataDiscoverer.",
                                        reflectionAttribute.Attribute.GetType(),
                                        this.TestCase.TestMethod.TestClass.Class.Name,
                                        this.TestCase.TestMethod.Method.Name
                                        )
                                    )
                                );
                        }
                        else
                        {
                            this.Aggregator.Add(
                                new InvalidOperationException(
                                    string.Format(
                                        CultureInfo.CurrentCulture,
                                        "A data discoverer specified on {0}.{1} does not implement IDataDiscoverer.",
                                        this.TestCase.TestMethod.TestClass.Class.Name,
                                        this.TestCase.TestMethod.Method.Name
                                        )
                                    )
                                );
                        }

                        continue;
                    }

                    IEnumerable<object[]> data;

                    if (discoverer is ServiceProviderMemberDataDiscoverer dataDiscoverer)
                    {
                       data = dataDiscoverer.GetData(dataAttribute, this.TestCase.TestMethod.Method, this.testEnvServiceProvider);
                    }
                    else
                    {
                        data = discoverer.GetData(dataAttribute, this.TestCase.TestMethod.Method);
                    }

                    if (data == null)
                    {
                        this.Aggregator.Add(
                            new InvalidOperationException(
                                string.Format(
                                    CultureInfo.CurrentCulture,
                                    "Test data returned null for {0}.{1}. Make sure it is statically initialized before this test method is called.",
                                    this.TestCase.TestMethod.TestClass.Class.Name,
                                    this.TestCase.TestMethod.Method.Name
                                )
                            )
                        );

                        continue;
                    }

                    foreach (var dataRow in data)
                    {
                        this.toDispose.AddRange(dataRow.OfType<IDisposable>());

                        ITypeInfo[] resolvedTypes = null;
                        var methodToRun = this.TestMethod;
                        var convertedDataRow = methodToRun.ResolveMethodArguments(dataRow);

                        if (methodToRun.IsGenericMethodDefinition)
                        {
                            resolvedTypes = this.TestCase.TestMethod.Method.ResolveGenericTypes(convertedDataRow);
                            methodToRun = methodToRun.MakeGenericMethod(resolvedTypes.Select(t => ((IReflectionTypeInfo)t).Type).ToArray());
                        }

                        var parameterTypes = methodToRun.GetParameters().Select(p => p.ParameterType).ToArray();
                        convertedDataRow = Reflector.ConvertArguments(convertedDataRow, parameterTypes);

                        var theoryDisplayName = this.TestCase.TestMethod.Method.GetDisplayNameWithArguments(this.DisplayName, convertedDataRow, resolvedTypes);
                        var test = this.CreateTest(this.TestCase, theoryDisplayName);
                        var skipReason = this.SkipReason ?? dataAttribute.GetNamedArgument<string>("Skip");
                        this.testRunners.Add(this.CreateTestRunner(test, this.MessageBus, this.TestClass, this.ConstructorArguments, methodToRun, convertedDataRow, skipReason, this.BeforeAfterAttributes, this.Aggregator, this.CancellationTokenSource));
                    }
                }
            }
            catch (Exception ex)
            {
                // Stash the exception so we can surface it during RunTestAsync
                this.dataDiscoveryException = ex;
            }
        }

    protected override async Task<RunSummary> RunTestAsync()
    {
        if (this.dataDiscoveryException != null)
        {
            return this.RunTest_DataDiscoveryException();
        }

        var runSummary = new RunSummary();
        foreach (var testRunner in this.testRunners)
        {
            runSummary.Aggregate(await testRunner.RunAsync());
        }

        // Run the cleanup here so we can include cleanup time in the run summary,
        // but save any exceptions so we can surface them during the cleanup phase,
        // so they get properly reported as test case cleanup failures.
        var timer = new ExecutionTimer();
        foreach (var disposable in this.toDispose)
        {
            timer.Aggregate(() => this.cleanupAggregator.Run(disposable.Dispose));
        }

        runSummary.Time += timer.Total;
        return runSummary;
    }

    RunSummary RunTest_DataDiscoveryException()
    {
        var test = new XunitTest(this.TestCase, this.DisplayName);

        if (!this.MessageBus.QueueMessage(new TestStarting(test)))
        {
            this.CancellationTokenSource.Cancel();
        }
        else if (!this.MessageBus.QueueMessage(new TestFailed(test, 0, null, this.dataDiscoveryException)))
        {
            this.CancellationTokenSource.Cancel();
        }

        if (!this.MessageBus.QueueMessage(new TestFinished(test, 0, null)))
        {
            this.CancellationTokenSource.Cancel();
        }

        return new RunSummary { Total = 1, Failed = 1 };
    }
}
