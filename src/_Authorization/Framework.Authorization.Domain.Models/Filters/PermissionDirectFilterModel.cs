using Framework.Restriction;

namespace Framework.Authorization.Domain;

public class PermissionDirectFilterModel : DomainObjectContextFilterModel<Permission>
{
    [Required]
    public SecurityContextType SecurityContextType { get; set; }

    [Required]
    public Guid SecurityContextId { get; set; }

    public bool StrongDirect { get; set; }
}
