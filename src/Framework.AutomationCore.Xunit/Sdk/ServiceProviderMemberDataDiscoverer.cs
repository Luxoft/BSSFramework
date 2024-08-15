using Xunit.Abstractions;
using Xunit.Sdk;

namespace Automation.Xunit.Sdk;

public class ServiceProviderMemberDataDiscoverer : IDataDiscoverer
{
    public IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod) =>
        throw new ArgumentException($"ServiceProviderMemberDataDiscoverer cannot be used as discoverer for any *DataAttribute other than ServiceProviderMemberDataAttribute.");

    public IEnumerable<object[]> GetData(IAttributeInfo dataAttribute, IMethodInfo testMethod, IServiceProvider serviceProvider)
    {
        if (dataAttribute is not IReflectionAttributeInfo reflectionDataAttribute
            || testMethod is not IReflectionMethodInfo reflectionTestMethod)
        {
            return null;
        }

        if (reflectionDataAttribute.Attribute is not ServiceProviderMemberDataAttribute attribute)
        {
            throw new ArgumentException($"ServiceProviderMemberDataDiscoverer cannot be used as discoverer for {reflectionDataAttribute.Attribute.GetType()}.");
        }

        try
        {
            return attribute.GetData(reflectionTestMethod.MethodInfo, serviceProvider);
        }
        catch (ArgumentException)
        {
            var reflectionTestMethodType = reflectionTestMethod.Type as IReflectionTypeInfo;
            if (attribute is { MemberType: null })
            {
                attribute.MemberType = reflectionTestMethodType?.Type;
            }

            return attribute.GetData(reflectionTestMethod.MethodInfo, serviceProvider);
        }
    }

    public bool SupportsDiscoveryEnumeration(IAttributeInfo dataAttribute, IMethodInfo testMethod) => false;
}
