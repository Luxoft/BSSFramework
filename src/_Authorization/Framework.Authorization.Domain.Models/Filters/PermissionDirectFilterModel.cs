using Framework.Restriction;

namespace Framework.Authorization.Domain;

public class PermissionDirectFilterModel : DomainObjectContextFilterModel<Permission>
{
    [Required]
    public SecurityContextType EntityType { get; set; }

    [Required]
    public Guid EntityId { get; set; }

    public bool StrongDirect { get; set; }
}
