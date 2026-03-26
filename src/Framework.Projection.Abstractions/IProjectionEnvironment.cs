using Framework.Core;

namespace Framework.Projection;

/// <summary>
/// Окружение проекций
/// </summary>
public interface IProjectionEnvironment
{
    /// <summary>
    /// Пространтсво имён
    /// </summary>
    string Namespace { get; }

    /// <summary>
    /// Сборка
    /// </summary>
    IAssemblyInfo Assembly { get; }

    /// <summary>
    /// Использование безопасности через атрибут `DependencySecurityAttribute`
    /// </summary>
    bool UseDependencySecurity { get; }
}
