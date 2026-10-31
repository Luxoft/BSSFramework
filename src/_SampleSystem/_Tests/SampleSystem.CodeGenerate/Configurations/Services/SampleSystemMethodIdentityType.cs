using Framework.CodeGeneration.ServiceModelGenerator;

namespace SampleSystem.CodeGenerate.Configurations.Services;

/// <summary>
/// Custom identifiers of generated facade methods
/// </summary>
public static class SampleSystemMethodIdentityType
{
    /// <summary>
    /// Identifier of facade method generation by a ComplexChange model
    /// </summary>
    public static readonly MethodIdentityType ComplexChange = new(nameof(ComplexChange));
}
