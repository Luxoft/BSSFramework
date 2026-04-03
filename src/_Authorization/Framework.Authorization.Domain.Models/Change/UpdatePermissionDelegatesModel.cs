namespace Framework.Authorization.Domain;

public class UpdatePermissionDelegatesModel : DomainObjectBase
{
    [Restriction.Required]
    public Permission DelegateFromPermission { get; set; }

    [Restriction.Required]
    public List<DelegateToItemModel> AddItems { get; set; } = new();

    [Restriction.Required]
    public List<Permission> RemoveItems { get; set; } = new();
}
