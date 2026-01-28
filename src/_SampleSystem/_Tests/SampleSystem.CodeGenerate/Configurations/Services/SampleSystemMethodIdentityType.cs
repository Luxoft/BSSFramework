using Framework.DomainDriven.ServiceModelGenerator;

namespace SampleSystem.CodeGenerate;

/// <summary>
/// Кастомные идентификаторы генерируемых фасадных методов
/// </summary>
public static class SampleSystemMethodIdentityType
{
    /// <summary>
    /// Идентификатор генерации фасадных методов по ComplexChange-модели
    /// </summary>
    public static readonly MethodIdentityType ComplexChange = new (nameof(ComplexChange));
}
