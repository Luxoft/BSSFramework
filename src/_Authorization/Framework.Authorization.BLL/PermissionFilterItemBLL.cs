using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.Authorization.BLL;

public partial class PermissionFilterItemBLL
{
    public void NotifySave(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.RaiseOperationProcessed(permissionFilterItem, BLLBaseOperation.Save);
    }

    public void NotifyRemove(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.RaiseOperationProcessed(permissionFilterItem, BLLBaseOperation.Remove);
    }
}
