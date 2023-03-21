using System;

namespace Framework.Events;

/// <summary>
/// Константы, описывающие тип события(event-a во внешнюю систему)
/// </summary>
public enum EventOperation
{
    /// <summary>
    /// Сохранение объекта
    /// </summary>
    Save = 0,

    /// <summary>
    /// Удаление объекта
    /// </summary>
    Remove = 1
}
