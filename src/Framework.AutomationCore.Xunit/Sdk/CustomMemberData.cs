using Xunit;
using Xunit.Sdk;

using System.Reflection;
using System.Runtime.CompilerServices;

namespace Automation.Xunit.Sdk;

[DataDiscoverer("Xunit.Sdk.MemberDataDiscoverer", "xunit.core")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class CustomMemberDataAttribute : MemberDataAttributeBase
{
    public CustomMemberDataAttribute(string memberName, params object[] parameters)
        : base(memberName, parameters) =>
        this.DisableDiscoveryEnumeration = true;

    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        RuntimeHelpers.RunClassConstructor(testMethod.ReflectedType!.TypeHandle);
        return base.GetData(testMethod);
    }

    protected override object[] ConvertDataItem(MethodInfo testMethod, object item) => (object[])item;
}
