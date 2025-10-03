using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;

using SecuritySystem.ExternalSystem.SecurityContextStorage;

namespace Framework.Authorization.BLL;

internal class PermissionDirectInternalFilterModel(IAuthorizationBLLContext context, PermissionDirectFilterModel baseFilterModel)
    : IDomainObjectFilterModel<Permission>
{
    public Expression<Func<Permission, bool>> ToFilterExpression()
    {
        var securityContextType = baseFilterModel.SecurityContextType;
        var securityContextId = baseFilterModel.SecurityContextId;

        if (baseFilterModel.StrongDirect)
        {
            return permission => permission.Restrictions.Any(filterItem => filterItem.SecurityContextType == securityContextType && filterItem.SecurityContextId == securityContextId);
        }
        else
        {
            var securityContextInfo = context.SecurityContextInfoSource.GetSecurityContextInfo(securityContextType.Id);

            var securityEntities = context.SecurityContextStorage.GetTyped<Guid>(securityContextInfo.Type).GetSecurityContextsWithMasterExpand(securityContextId);

            var entityIdents = securityEntities.ToList(se => se.Id);

            return permission => permission.Restrictions.All(filterItem => filterItem.SecurityContextType != securityContextType)
                                 || permission.Restrictions.Any(filterItem => entityIdents.Contains(filterItem.SecurityContextId));
        }
    }
}
