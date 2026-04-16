using Xunit;
using Xunit.Sdk;

namespace Microsoft.VisualStudio.TestTools.UnitTesting;

[AttributeUsage(AttributeTargets.Class)]
public sealed class TestClassAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestMethodAttribute : FactAttribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DataTestMethodAttribute : TheoryAttribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestInitializeAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class TestCleanupAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AssemblyInitializeAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class AssemblyCleanupAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class IgnoreAttribute : Attribute;

[AttributeUsage(AttributeTargets.Method)]
public sealed class DescriptionAttribute(string description) : Attribute
{
    public string Description { get; } = description;
}

public enum DynamicDataSourceType
{
    Method,
    Property,
}

public static class Assert
{
    public static void AreEqual<T>(T expected, T actual, string? message = null)
    {
        try
        {
            Xunit.Assert.Equal(expected, actual);
        }
        catch (EqualException) when (message != null)
        {
            throw new XunitException(message);
        }
    }

    public static void IsTrue(bool condition, string? message = null)
    {
        try
        {
            Xunit.Assert.True(condition);
        }
        catch (TrueException) when (message != null)
        {
            throw new XunitException(message);
        }
    }

    public static void IsNotNull(object? value, string? message = null)
    {
        try
        {
            Xunit.Assert.NotNull(value);
        }
        catch (Xunit.Sdk.NotNullException) when (message != null)
        {
            throw new XunitException(message);
        }
    }

    public static void Fail(string? message = null) => throw new XunitException(message ?? "Assertion failed.");
}
