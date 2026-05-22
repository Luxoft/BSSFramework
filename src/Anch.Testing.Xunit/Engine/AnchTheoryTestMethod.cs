using System.Reflection;

using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit.Engine;

public class AnchTheoryTestMethod(IXunitTestMethod baseMethod, IServiceProviderPool? serviceProviderPool) : IXunitTestMethod, IXunitSerializable
{
    public int? MethodArity => baseMethod.MethodArity;

    public string MethodName => baseMethod.MethodName;

    public IReadOnlyDictionary<string, IReadOnlyCollection<string>> Traits => baseMethod.Traits;

    public string UniqueID => baseMethod.UniqueID;

    public IReadOnlyCollection<IBeforeAfterTestAttribute> BeforeAfterTestAttributes
        => baseMethod.BeforeAfterTestAttributes;

    public IReadOnlyCollection<IDataAttribute> DataAttributes => field ??=
    [
        .. baseMethod.DataAttributes.Select(attr =>
        {
            if (attr is AnchMemberDataAttribute commonMemberDataAttribute)
            {
                commonMemberDataAttribute.ServiceProviderPool = serviceProviderPool;
            }

            return attr;
        })
    ];

    public IReadOnlyCollection<IFactAttribute> FactAttributes => baseMethod.FactAttributes;

    public bool IsGenericMethodDefinition
        => baseMethod.IsGenericMethodDefinition;

    public MethodInfo Method
        => baseMethod.Method;

    public IReadOnlyCollection<ParameterInfo> Parameters
        => baseMethod.Parameters;

    public Type ReturnType
        => baseMethod.ReturnType;

    public object?[] TestMethodArguments
        => baseMethod.TestMethodArguments;

    public IXunitTestClass TestClass
        => baseMethod.TestClass;

    ITestClass ITestMethod.TestClass
        => this.TestClass;

    public string GetDisplayName(
        string baseDisplayName,
        string? label,
        object?[]? testMethodArguments,
        Type[]? methodGenericTypes)
        => baseMethod.GetDisplayName(baseDisplayName, label, testMethodArguments, methodGenericTypes);

    public MethodInfo MakeGenericMethod(Type[] genericTypes)
        => baseMethod.MakeGenericMethod(genericTypes);

    public Type[]? ResolveGenericTypes(object?[] arguments)
        => baseMethod.ResolveGenericTypes(arguments);

    public object?[] ResolveMethodArguments(object?[] arguments)
        => baseMethod.ResolveMethodArguments(arguments);

    public void Deserialize(IXunitSerializationInfo info)
    {
        (baseMethod as IXunitSerializable)?.Deserialize(info);
    }

    public void Serialize(IXunitSerializationInfo info)
    {
        (baseMethod as IXunitSerializable)?.Serialize(info);
    }
}