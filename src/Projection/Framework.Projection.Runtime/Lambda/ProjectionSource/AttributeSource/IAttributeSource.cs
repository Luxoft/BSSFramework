namespace Framework.Projection.Lambda.ProjectionSource.AttributeSource;

/// <summary>
/// Источник атрибутов для типов/свойств проекций
/// </summary>
public interface IAttributeSource
{
    /// <summary>
    /// Получение всех атрибутов объекта
    /// </summary>
    /// <returns></returns>
    IEnumerable<Attribute> GetAttributes();
}
