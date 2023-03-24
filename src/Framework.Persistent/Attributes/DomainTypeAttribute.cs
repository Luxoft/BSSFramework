using System;

using Framework.Core;

namespace Framework.Persistent;

/// <summary>
/// Атрибут маркировки доменного типа
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DomainTypeAttribute : Attribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="id">Идентификатор типа</param>
    public DomainTypeAttribute(string id)
    {
        this.Id = new Guid(id);

        if (this.Id.IsDefault())
        {
            throw new ArgumentOutOfRangeException(nameof(id), "empty id");
        }
    }

    /// <summary>
    /// Идентификатор типа
    /// </summary>
    public Guid Id { get; }
}
