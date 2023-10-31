using Automation.Xunit.Interfaces;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Sdk;

public class AutomationCoreTheoryDiscoverer : TheoryDiscoverer
{
    public AutomationCoreTheoryDiscoverer(IMessageSink diagnosticMessageSink)
        : base(diagnosticMessageSink)
    {
    }

    protected override IEnumerable<IAutomationCoreTheoryTestCase> CreateTestCasesForTheory(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo theoryAttribute)
        => new[]
           {
               new AutomationCoreTheoryTestCase(
                   this.DiagnosticMessageSink,
                   discoveryOptions.MethodDisplayOrDefault(),
                   discoveryOptions.MethodDisplayOptionsOrDefault(),
                   testMethod)
           };

    protected override IEnumerable<IXunitTestCase> CreateTestCasesForDataRow(
        ITestFrameworkDiscoveryOptions discoveryOptions,
        ITestMethod testMethod,
        IAttributeInfo theoryAttribute,
        object[] dataRow)
        => new[]
           {
               new AutomationCoreTestCase(
                   this.DiagnosticMessageSink,
                   discoveryOptions.MethodDisplayOrDefault(),
                   discoveryOptions.MethodDisplayOptionsOrDefault(),
                   testMethod,
                   dataRow)
           };
}

