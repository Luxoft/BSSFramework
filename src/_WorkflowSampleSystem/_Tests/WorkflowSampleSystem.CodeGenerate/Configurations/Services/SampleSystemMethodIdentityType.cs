using Framework.DomainDriven.ServiceModelGenerator;

namespace WorkflowSampleSystem.CodeGenerate
{
    /// <summary>
    /// Кастомные идентификаторы генерируемых фасадных методов
    /// </summary>
    public static class WorkflowSampleSystemMethodIdentityType
    {
        /// <summary>
        /// Идентификатор генерации фасадных методов по ComplexChange-модели
        /// </summary>
        public static readonly MethodIdentityType ComplexChange = new MethodIdentityType(() => ComplexChange);
    }
}
