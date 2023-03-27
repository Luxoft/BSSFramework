using JetBrains.Annotations;

namespace Framework.Projection;

/// <summary>
/// Атрибут указывающий, что данный тип является фильтром для проекции
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ProjectionFilterAttribute : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="filterType">Тип фильтра</param>
    /// <param name="target">Применимость фильтра</param>
    public ProjectionFilterAttribute([NotNull] Type filterType, ProjectionFilterTargets target)
    {
        this.FilterType = filterType ?? throw new ArgumentNullException(nameof(filterType));
        this.Target = target;
    }

    /// <summary>
    /// Тип фильтра
    /// </summary>
    public Type FilterType { get; }

    /// <summary>
    /// Применимость фильтра
    /// </summary>
    public ProjectionFilterTargets Target { get; }
}
