using Framework.BLL.Domain.Serialization;

namespace Framework.BLL.Domain.DTO;

/// <summary>
/// Generated type attribute
/// </summary>
public class DTOFileTypeAttribute(Type domainType, string name, DTORole role) : Attribute
{
    /// <summary>
    /// Domain type
    /// </summary>
    public Type DomainType { get; } = domainType;

    /// <summary>
    /// Generated type
    /// </summary>
    public string Name { get; } = name;

    /// <summary>
    /// Role
    /// </summary>
    public DTORole Role { get; } = role;

    /// <summary>
    /// Additional generation data
    /// </summary>
    public string? ExternalData { get; init; }
}
