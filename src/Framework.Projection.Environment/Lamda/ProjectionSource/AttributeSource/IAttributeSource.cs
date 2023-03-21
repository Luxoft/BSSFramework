using System;
using System.Collections.Generic;

namespace Framework.Projection.Lambda;

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
