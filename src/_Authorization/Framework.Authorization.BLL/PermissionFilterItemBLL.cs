using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.BLL;

public partial class PermissionFilterItemBLL
{
    public void NotifySave(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.Context.OperationSender.Send(permissionFilterItem, DomainObjectEvent.Save);
    }

    public void NotifyRemove(PermissionFilterItem permissionFilterItem)
    {
        if (permissionFilterItem == null) throw new ArgumentNullException(nameof(permissionFilterItem));

        this.Context.OperationSender.Send(permissionFilterItem, DomainObjectEvent.Remove);
    }
}
