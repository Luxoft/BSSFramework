using Anch.Testing.Xunit.Engine;

using Xunit.v3;

namespace Anch.Testing.Xunit;

[AttributeUsage(AttributeTargets.Assembly)]
public class AnchTestFrameworkAttribute : Attribute, ITestFrameworkAttribute
{
    public Type FrameworkType { get; } = typeof(AnchTestFramework);

    public virtual Type? TestEnvironmentType { get; } = null;
}

public class AnchTestFrameworkAttribute<TTestEnvironment> : AnchTestFrameworkAttribute
    where TTestEnvironment : ITestEnvironment
{
    public override Type TestEnvironmentType { get; } = typeof(TTestEnvironment);
}