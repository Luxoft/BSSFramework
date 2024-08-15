using System.Collections;
using System.Globalization;

using Xunit.Sdk;

using System.Reflection;

namespace Automation.Xunit.Sdk;

[DataDiscoverer("Automation.Xunit.Sdk.ServiceProviderMemberDataDiscoverer", "Framework.AutomationCore.Xunit")]
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class ServiceProviderMemberDataAttribute(string methodName) : DataAttribute
{
    public Type MemberType { get; set; }

    string MemberName { get; set; } = methodName;

    public override IEnumerable<object[]> GetData(MethodInfo testMethod) => null;

    public IEnumerable<object[]> GetData(MethodInfo testMethod, IServiceProvider serviceProvider)
    {
        var type = this.MemberType ?? testMethod.DeclaringType;
        var accessor = this.GetMethodAccessor(type, serviceProvider);
        if (accessor == null)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "Could not find public static method named '{0}' on {1}{2}",
                    this.MemberName,
                    type?.FullName,
                    " with parameter types: IServiceProvider")
                );
        }

        var obj = accessor();
        if (obj == null)
        {
            return null;
        }

        if (obj is not IEnumerable dataItems)
        {
            throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Method {0} on {1} did not return IEnumerable", this.MemberName, type?.FullName));
        }

        return dataItems.Cast<object>().Select(item => this.ConvertDataItem(testMethod, item));
    }

    protected Func<object> GetMethodAccessor(Type type, IServiceProvider serviceProvider)
    {
        MethodInfo methodInfo = null;
        for (var reflectionType = type; reflectionType != null; reflectionType = reflectionType.GetTypeInfo().BaseType)
        {
            var runtimeMethodsWithGivenName = reflectionType.GetRuntimeMethods()
                                                            .Where(m => m.Name == this.MemberName)
                                                            .ToArray();

            methodInfo = runtimeMethodsWithGivenName
                .FirstOrDefault(m => m.GetParameters()
                                      .Count(x => x.ParameterType.IsAssignableTo(typeof(IServiceProvider))) == 1);

            if (methodInfo != null)
            {
                break;
            }
        }

        if (methodInfo == null || !methodInfo.IsStatic)
        {
            return null;
        }

        return () => methodInfo.Invoke(null, [serviceProvider]);
    }

    protected object[] ConvertDataItem(MethodInfo testMethod, object item)
    {
        if (item == null)
        {
            return null;
        }

        if (item is not object[] array)
        {
            throw new ArgumentException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "Method {0} on {1} yielded an item that is not an object[]",
                    this.MemberName,
                    this.MemberType ?? testMethod.DeclaringType
                    ));
        }

        return array;
    }
}
