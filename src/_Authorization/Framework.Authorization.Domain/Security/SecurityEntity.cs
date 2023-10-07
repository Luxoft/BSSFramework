using Framework.Core;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

/// <summary>
/// Контексты, в разрезе которых выдаются права
/// </summary>
public class SecurityEntity : DomainObjectBase, IDefaultIdentityObject, ISecurityVisualIdentityObject, IVisualIdentityObject, IActiveObject, IEquatable<SecurityEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    public SecurityEntity()
    {
    }

    public Guid Id { get; set; }

    public bool Active { get; set; }

    public string Name { get; set; }

    public Guid ParentId { get; set; }

    public static bool operator ==(SecurityEntity v1, SecurityEntity v2)
    {
        return object.ReferenceEquals(v1, v2)
               || (!object.ReferenceEquals(v1, null) && v1.Equals(v2));
    }

    public static bool operator !=(SecurityEntity v1, SecurityEntity v2)
    {
        return !(v1 == v2);
    }

    public override string ToString()
    {
        return this.Name;
    }

    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as SecurityEntity);
    }

    public bool Equals(SecurityEntity other)
    {
        return other.Maybe(v => v.Id == this.Id);
    }
}
