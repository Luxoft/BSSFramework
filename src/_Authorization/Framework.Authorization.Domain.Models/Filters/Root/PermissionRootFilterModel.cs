namespace Framework.Authorization.Domain;

public class PermissionRootFilterModel : DomainObjectRootFilterModel<Permission>
{
    public Permission DelagetedFrom { get; set; }

    public Principal Principal { get; set; }

    public SecurityContextType SecurityContextType { get; set; }

    public Guid? SecurityContextId { get; set; }

    public override System.Linq.Expressions.Expression<Func<Permission, bool>> ToFilterExpression()
    {
        var principal = this.Principal;

        var securityContextType = this.SecurityContextType;

        var delagetedFrom = this.DelagetedFrom;

        var securityContextId = this.SecurityContextId;

        return permission => (principal == null || permission.Principal == principal)
                             && (delagetedFrom == null || delagetedFrom == permission.DelegatedFrom)
                             && (securityContextType == null
                                 || permission.Restrictions.Any(
                                     restriction =>

                                         restriction.SecurityContextType == securityContextType
                                         && (securityContextId == null || restriction.SecurityContextId == securityContextId)
                                     ));
    }
}
