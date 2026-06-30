using Anch.Core;

using Framework.BLL.Domain.DTO;

namespace Framework.CodeGeneration.ServiceModelGenerator;

public class MethodIdentity(MethodIdentityType type, Type? modelType = null, ViewDTOType? dtoType = null) : IEquatable<MethodIdentity>
{
    public MethodIdentity(MethodIdentityType type, ViewDTOType dtoType)
        : this(type, null, dtoType)
    {
    }

    public Type? ModelType { get; } = modelType;

    public ViewDTOType? DTOType { get; } = dtoType;

    public MethodIdentityType Type { get; } = type ?? throw new ArgumentNullException(nameof(type));

    public virtual bool Equals(MethodIdentity? other) =>
        other is not null && this.Type == other.Type && this.ModelType == other.ModelType && this.DTOType == other.DTOType;

    public override bool Equals(object? obj) => this.Equals(obj as MethodIdentity);

    public override int GetHashCode() => this.Type.GetHashCode();

    public override string ToString() => this.Type.Name;

    public static implicit operator MethodIdentity(MethodIdentityType type) => type.Maybe(v => new MethodIdentity(v))!;

    public static bool operator ==(MethodIdentity source, MethodIdentity other) =>
        ReferenceEquals(source, other)
        || (source is not null && source.Equals(other));

    public static bool operator !=(MethodIdentity fileType, MethodIdentity other) => !(fileType == other);
}
