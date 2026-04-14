using Framework.BLL.Domain.DirectMode;
using Framework.BLL.Domain.Models;

namespace SampleSystem.CodeGenerate.Configurations.Services;

/// <summary>
/// Кастомные роли SampleSystem-моделей
/// </summary>
public static class SampleSystemModelRole
{
    /// <summary>
    /// Роль моделей для генерации изменений коллекции объектов
    /// </summary>
    public static readonly ModelRole ComplexChange = new ModelRole(() => ComplexChange, DirectMode.In);
}
