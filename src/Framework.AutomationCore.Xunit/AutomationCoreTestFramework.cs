using System.Reflection;

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit;

public class AutomationCoreTestFramework : XunitTestFramework
{
    public AutomationCoreTestFramework(IMessageSink messageSink) : base(messageSink)
    {
    }

    protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName) =>
        new AutomationCoreFrameworkExecutor(assemblyName, this.SourceInformationProvider, this.DiagnosticMessageSink);
}
