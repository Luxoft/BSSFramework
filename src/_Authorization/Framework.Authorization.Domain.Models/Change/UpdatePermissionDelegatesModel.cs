namespace Framework.Authorization.Domain;

public class UpdatePermissionDelegatesModel : DomainObjectBase
{
    public UpdatePermissionDelegatesModel()
    {
        this.AddItems = new List<DelegateToItemModel>();
        this.RemoveItems = new List<Permission>();
    }

    [Restriction.Required]
    public Permission DelegateFromPermission { get; set; }

    [Restriction.Required]
    public IList<DelegateToItemModel> AddItems { get; set; }

    [Restriction.Required]
    public IList<Permission> RemoveItems { get; set; }
}
