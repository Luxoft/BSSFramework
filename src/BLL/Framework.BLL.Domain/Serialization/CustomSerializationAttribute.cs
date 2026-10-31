namespace Framework.BLL.Domain.Serialization;

/// <summary>
/// Allows configuring serialization of domain type properties into a specific DTO type
/// By default, all suitable DTO types are generated for all domain types, along with the corresponding mapping from domain to DTO or from DTO to domain (depending on the DTO kind - input or output).
/// <see href="confluence/display/IADFRAME/CustomSerializationAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = true)]
public class CustomSerializationAttribute(CustomSerializationMode mode, DTORole role) : Attribute
{
    public readonly CustomSerializationMode Mode = mode;

    public readonly DTORole Role = role;

    public CustomSerializationAttribute(CustomSerializationMode mode)
            : this(mode, DTORole.All)
    {
    }
}
