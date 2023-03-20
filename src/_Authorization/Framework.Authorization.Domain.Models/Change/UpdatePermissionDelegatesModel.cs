using System.Collections.Generic;
using Framework.Validation;

namespace Framework.Authorization.Domain;

public class UpdatePermissionDelegatesModel : DomainObjectBase
{
    public UpdatePermissionDelegatesModel()
    {
        this.AddItems = new List<DelegateToItemModel>();
        this.RemoveItems = new List<Permission>();
    }

    [Framework.Restriction.Required]
    public Permission DelegateFromPermission { get; set; }

    [Framework.Restriction.Required]
    public IList<DelegateToItemModel> AddItems { get; set; }

    [Framework.Restriction.Required]
    public IList<Permission> RemoveItems { get; set; }
}
