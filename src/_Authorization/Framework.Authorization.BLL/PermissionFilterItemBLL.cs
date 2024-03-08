using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.BLL;

public partial class PermissionFilterItemBLL
{
    public void NotifySave(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.RaiseOperationProcessed(permissionFilterItem, EventOperation.Save);
    }

    public void NotifyRemove(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.RaiseOperationProcessed(permissionFilterItem, EventOperation.Remove);
    }
}
