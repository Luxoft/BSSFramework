using System.Reflection;

using Xunit;
using Xunit.Sdk;
using Xunit.v3;

namespace Anch.Testing.Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AnchInlineDataAttribute(params object?[]? data) : DataAttribute
{
    private readonly InlineDataAttribute innerAttr = new(data);

    /// <inheritdoc/>
    public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(
        MethodInfo testMethod,
        DisposalTracker disposalTracker) => this.innerAttr.GetData(testMethod, disposalTracker);

    /// <inheritdoc/>
    public override bool SupportsDiscoveryEnumeration() => this.innerAttr.SupportsDiscoveryEnumeration();
}