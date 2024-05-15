using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
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

        base.Save(principal);
    }

    protected override void Validate(Principal domainObject, AuthorizationOperationContext operationContext)
    {
        this.Context.PrincipalValidator.Validate(domainObject);

        base.Validate(domainObject, operationContext);
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

    private Principal GetActiveByName(string name)
    {
        var result = this.GetObjectBy(p => p.Name == name && p.Active);
        return result;
    }
}
