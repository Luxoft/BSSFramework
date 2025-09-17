using CommonFramework;

using Framework.Core;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class MethodIdentity : IEquatable<MethodIdentity>
{
    public MethodIdentity(MethodIdentityType type, ViewDTOType dtoType)
            : this(type, null, dtoType)
    {
    }

    public MethodIdentity(MethodIdentityType type, Type modelType = null, ViewDTOType? dtoType = null)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        this.Type = type;
        this.ModelType = modelType;
        this.DTOType = dtoType;
    }


    public Type ModelType { get; }

    public ViewDTOType? DTOType { get; }

    public MethodIdentityType Type { get; }


    public virtual bool Equals(MethodIdentity other)
    {
        return !ReferenceEquals(other, null) && this.Type == other.Type && this.ModelType == other.ModelType && this.DTOType == other.DTOType;
    }

    public override bool Equals(object obj)
    {
        return this.Equals(obj as MethodIdentity);
    }

    public override int GetHashCode()
    {
        return this.Type.GetHashCode();
    }

    public override string ToString()
    {
        return this.Type.Name;
    }


    public static implicit operator MethodIdentity(MethodIdentityType type)
    {
        return type.Maybe(v => new MethodIdentity(v));
    }

    public static bool operator == (MethodIdentity source, MethodIdentity other)
    {
        return ReferenceEquals(source, other)
               || (!ReferenceEquals(source, null) && source.Equals(other));
    }

    public static bool operator !=(MethodIdentity fileType, MethodIdentity other)
    {
        return !(fileType == other);
    }
}
