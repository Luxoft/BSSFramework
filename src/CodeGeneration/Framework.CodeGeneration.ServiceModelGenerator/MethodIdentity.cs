using CommonFramework;

using Framework.BLL.Domain.DTO;

namespace Framework.CodeGeneration.ServiceModelGenerator;

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


    public virtual bool Equals(MethodIdentity other) => !ReferenceEquals(other, null) && this.Type == other.Type && this.ModelType == other.ModelType && this.DTOType == other.DTOType;

    public override bool Equals(object obj) => this.Equals(obj as MethodIdentity);

    public override int GetHashCode() => this.Type.GetHashCode();

    public override string ToString() => this.Type.Name;

    public static implicit operator MethodIdentity(MethodIdentityType type) => type.Maybe(v => new MethodIdentity(v));

    public static bool operator == (MethodIdentity source, MethodIdentity other) =>
        ReferenceEquals(source, other)
        || (!ReferenceEquals(source, null) && source.Equals(other));

    public static bool operator !=(MethodIdentity fileType, MethodIdentity other) => !(fileType == other);
}
