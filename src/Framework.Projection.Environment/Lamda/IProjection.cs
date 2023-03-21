using System;
using System.Collections.Generic;

namespace Framework.Projection.Lambda;

/// <summary>
/// Интерфейс проекции
/// </summary>
public interface IProjection : IProjectionAttributeProvider
{
    /// <summary>
    /// Тип на основе которого строится проекциея
    /// </summary>
    Type SourceType { get; }

    /// <summary>
    /// Имя проекции
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Генерация BLLViewRole логики и фасадов
    /// </summary>
    bool BLLView { get; }

    /// <summary>
    /// Роль проекции
    /// </summary>
    ProjectionRole Role { get; }

    /// <summary>
    /// Список свойств
    /// </summary>
    IReadOnlyList<IProjectionProperty> Properties { get; }

    /// <summary>
    /// Список расчётных свойств
    /// </summary>
    IReadOnlyList<IProjectionCustomProperty> CustomProperties { get; }

    /// <summary>
    /// Дополнительные фильтры
    /// </summary>
    IReadOnlyList<ProjectionFilterAttribute> FilterAttributes { get; }

    /// <summary>
    /// Игнорирование Id-свойства при сереализации
    /// </summary>
    bool IgnoreIdSerialization { get; }
}
