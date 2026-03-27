namespace Framework.Core.TypeResolving.TypeSource;

/// <summary>
/// Источник типов
/// </summary>
public interface ITypeSource
{
    /// <summary>
    /// Получение типов
    /// </summary>
    /// <returns></returns>
    IEnumerable<Type> GetTypes();
}
