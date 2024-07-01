using System.Linq.Expressions;

using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityRule.DomainObjectSecurityRule securityRule,
        Expression<Func<IPermission<TIdent>, bool>> permissionFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityRule.DomainObjectSecurityRule securityRule,
        IEnumerable<Type> securityTypes);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityRule.DomainObjectSecurityRule securityRule);
}
