using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.Tracking;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
    public Principal Create(PrincipalCreateModel createModel)
    {
        return new Principal();
    }

    public override void Save(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        foreach (var permission in principal.Permissions)
        {
            var removedSelfDelegatePermissions =

                    permission.DelegatedTo
                              .Where(toPermission => toPermission.DelegatedFromPrincipal == principal
                                                     && !principal.Permissions.Contains(toPermission))
                              .ToList();

            permission.RemoveDetails(removedSelfDelegatePermissions);
        }

        this.PermissionRestrictionNotifyProgress(principal);

        base.Save(principal);

        this.NotifySaveAndRemove(principal);
    }

    public Principal GetByNameOrCreate(string name, bool autoSave = false)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.GetActiveByName(name) ?? new Principal { Name = name }.Self(autoSave, this.Save);
    }

    public Principal GetCurrent(bool autoSave = false)
    {
        return this.GetByNameOrCreate(this.Context.CurrentPrincipalName, autoSave);
    }

    public override void Remove(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        this.Remove(principal, false);
    }

    public void Remove(Principal principal, bool force)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        if (force)
        {
            principal.Permissions.Foreach(p => p.DelegatedTo.Foreach(delP => delP.Principal.RemoveDetail(delP)));
        }
        else if (principal.Permissions.Any())
        {
            throw new BusinessLogicException($"Removing principal \"{principal.Name}\" must be empty");
        }

        base.Remove(principal);
    }

    protected override void PostValidate(Principal principal, AuthorizationOperationContext operationContext)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        foreach (var permission in principal.Permissions)
        {
            this.Context.PermissionValidator.Validate(permission);
        }
    }

    private void NotifySaveAndRemove(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        principal.Permissions.Foreach(permission => this.Context.Logics.Permission.Save(permission, false));

        this.Context.TrackingService.GetChanges(principal).GetChange(p => p.Permissions).Match(trackProp =>
                trackProp.ToMergeResult().RemovingItems.Foreach(this.Context.Logics.Permission.Remove));
    }

    private void PermissionRestrictionNotifyProgress(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        var filterItemBLL = this.Context.Logics.PermissionRestriction;

        var prevFilterItems = filterItemBLL.GetListBy(filterItem => filterItem.Permission.Principal == principal);

        var currentFilterItems = principal.Permissions.SelectMany(permission => permission.Restrictions).ToList();

        var mergeResult = prevFilterItems.GetMergeResult(currentFilterItems);

        mergeResult.RemovingItems.Foreach(filterItemBLL.NotifyRemove);

        mergeResult.AddingItems.Foreach(filterItemBLL.NotifySave);
    }

    private Principal GetActiveByName(string name)
    {
        var result = this.GetObjectBy(p => p.Name == name && p.Active);
        return result;
    }
}
