namespace Framework.Core;

/// <summary>
/// Данные о сборке
/// </summary>
public interface IAssemblyInfo : ITypeSource
{
    /// <summary>
    /// Имя сборки
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Полное имя сборки
    /// </summary>
    string FullName { get; }
}
