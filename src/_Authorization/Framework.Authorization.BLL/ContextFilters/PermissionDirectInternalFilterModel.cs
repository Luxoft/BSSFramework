using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Core;

namespace Framework.Authorization.BLL;

internal class PermissionDirectInternalFilterModel : DomainObjectFilterModel<Permission>
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

    public override Expression<Func<Permission, bool>> ToFilterExpression()
    {
        var entityType = this.baseFilterModel.EntityType;
        var entityId = this.baseFilterModel.EntityId;

        if (this.baseFilterModel.StrongDirect)
        {
            return permission => permission.Restrictions.Any(filterItem => filterItem.SecurityContextType == entityType && filterItem.SecurityContextId == entityId);
        }
        else
        {
            var securityEntities = this.context.ExternalSource.GetTyped(entityType).GetSecurityEntitiesWithMasterExpand(entityId);

            var enitityIdents = securityEntities.ToList(se => se.Id);

            return permission => permission.Restrictions.All(filterItem => filterItem.SecurityContextType != entityType)
                                 || permission.Restrictions.Any(filterItem => enitityIdents.Contains(filterItem.SecurityContextId));
        }
    }
}
