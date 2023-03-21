using System;

namespace Framework.DomainDriven.ProjectionGenerator;

/// <summary>
/// Тип проекции
/// </summary>
public enum FileType
{
    /// <summary>
    /// Обычная проекция
    /// </summary>
    Projection,

    /// <summary>
    /// Базовая проекция с набором абстрактных кастомных свойств
    /// </summary>
    CustomProjectionBase
}
