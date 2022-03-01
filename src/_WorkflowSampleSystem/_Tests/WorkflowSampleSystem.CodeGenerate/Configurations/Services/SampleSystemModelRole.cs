using Framework.DomainDriven;

namespace WorkflowSampleSystem.CodeGenerate
{
    /// <summary>
    /// Кастомные роли WorkflowSampleSystem-моделей
    /// </summary>
    public static class WorkflowSampleSystemModelRole
    {
        /// <summary>
        /// Роль моделей для генерации изменений коллекции объектов
        /// </summary>
        public static readonly ModelRole ComplexChange = new ModelRole(() => ComplexChange, DirectMode.In);
    }
}
