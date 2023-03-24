using System;

using JetBrains.Annotations;

namespace Framework.Projection.Contract;

/// <summary>
/// Атрибут указывающий, что данный интерфейс является проекцией
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class ProjectionContractAttribute : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="sourceType">Исходный тип, по которому будет строиться проекция</param>
    public ProjectionContractAttribute([NotNull] Type sourceType)
    {
        if (sourceType == null) throw new ArgumentNullException(nameof(sourceType));

        if (sourceType.IsAbstract)
        {
            throw new Exception($"Type {sourceType.Name} can't be abstract");
        }

        if (sourceType.IsGenericTypeDefinition)
        {
            throw new Exception($"Type {sourceType.Name} can't be generic");
        }

        this.SourceType = sourceType;
    }

    /// <summary>
    /// Исходный тип, по которому будет строиться проекция
    /// </summary>
    public Type SourceType { get; }
}
