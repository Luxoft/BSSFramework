using Framework.Authorization.Domain;
using Framework.Events;

namespace Framework.Authorization.BLL;

public partial class PermissionRestrictionBLL
{
    public void NotifySave(PermissionRestriction permissionRestriction)
    {
        if (permissionRestriction == null) throw new ArgumentNullException(nameof(permissionRestriction));

        this.Context.OperationSender.Send(permissionRestriction, EventOperation.Save);
    }

    public void NotifyRemove(PermissionRestriction permissionRestriction)
    {
        if (permissionRestriction == null) throw new ArgumentNullException(nameof(permissionRestriction));

        this.Context.OperationSender.Send(permissionRestriction, EventOperation.Remove);
    }
}
