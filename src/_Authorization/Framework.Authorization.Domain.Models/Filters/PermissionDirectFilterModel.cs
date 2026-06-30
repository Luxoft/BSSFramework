using Framework.Restriction;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.Domain;

public class PermissionDirectFilterModel : DomainObjectContextFilterModel<Permission>
{
    [Required]
    public SecurityContextType SecurityContextType { get; set; } = null!;

    [Required]
    public Guid SecurityContextId { get; set; }

    public bool StrongDirect { get; set; }
}

