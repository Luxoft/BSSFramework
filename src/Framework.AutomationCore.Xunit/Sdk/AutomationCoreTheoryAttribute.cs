using Xunit;
using Xunit.Sdk;

namespace Automation.Xunit.Sdk;

[XunitTestCaseDiscoverer("Automation.Xunit.Sdk.AutomationCoreTheoryDiscoverer", "Framework.AutomationCore.Xunit")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class AutomationCoreTheoryAttribute : FactAttribute
{
}
