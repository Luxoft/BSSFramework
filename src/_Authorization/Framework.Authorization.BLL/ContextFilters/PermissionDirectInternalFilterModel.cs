using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.DomainDriven;

namespace Framework.Authorization.BLL;

internal class PermissionDirectInternalFilterModel : IDomainObjectFilterModel<Permission>
{
    private readonly IAuthorizationBLLContext context;
    private readonly PermissionDirectFilterModel baseFilterModel;

    public PermissionDirectInternalFilterModel(IAuthorizationBLLContext context, PermissionDirectFilterModel baseFilterModel)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (baseFilterModel == null) throw new ArgumentNullException(nameof(baseFilterModel));

        this.context = context;
        this.baseFilterModel = baseFilterModel;
    }

    public Expression<Func<Permission, bool>> ToFilterExpression()
    {
        var securityContextType = this.baseFilterModel.SecurityContextType;
        var securityContextId = this.baseFilterModel.SecurityContextId;

        if (this.baseFilterModel.StrongDirect)
        {
            return permission => permission.Restrictions.Any(filterItem => filterItem.SecurityContextType == securityContextType && filterItem.SecurityContextId == securityContextId);
        }
        else
        {
            var securityContextInfo = this.context.SecurityContextInfoSource.GetSecurityContextInfo(securityContextType.Id);
            
            throw new Exception("Use CommonFramework");

            //var securityEntities = this.context.SecurityContextStorage.GetTyped(securityContextInfo.Type).GetSecurityContextsWithMasterExpand(securityContextId);

            //var entityIdents = securityEntities.ToList(se => se.Id);

            //return permission => permission.Restrictions.All(filterItem => filterItem.SecurityContextType != securityContextType)
            //                     || permission.Restrictions.Any(filterItem => entityIdents.Contains(filterItem.SecurityContextId));
        }
    }
}
