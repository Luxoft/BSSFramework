using System;
using System.ComponentModel;

using JetBrains.Annotations;

namespace Framework.Projection;

/// <summary>
/// Атрибут указывающий, что данный тип является проекцией
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class ProjectionAttribute : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="sourceType">Исходный тип, по которому строилась проекция</param>
    /// <param name="role">Роль проекции</param>
    /// <param name="contractType">Контракт, по которому строилась проекция</param>
    public ProjectionAttribute([NotNull] Type sourceType, ProjectionRole role, Type contractType = null)
    {
        if (!Enum.IsDefined(typeof(ProjectionRole), role)) { throw new InvalidEnumArgumentException(nameof(role), (int)role, typeof(ProjectionRole)); }

        this.SourceType = sourceType ?? throw new ArgumentNullException(nameof(sourceType));
        this.Role = role;
        this.ContractType = contractType;
    }

    /// <summary>
    /// Исходный тип, по которому строилась проекция
    /// </summary>
    public Type SourceType { get; }

    /// <summary>
    /// Роль проекции
    /// </summary>
    public ProjectionRole Role { get; }

    /// <summary>
    /// Контракт, по которому строилась проекция
    /// </summary>
    public Type ContractType { get; }
}
