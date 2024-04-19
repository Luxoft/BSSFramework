﻿using System.Linq.Expressions;

using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem;

public interface IAuthorizationSystem<TIdent> : IAuthorizationSystem
{
    IEnumerable<string> GetNonContextAccessors(
        SecurityRule.DomainObjectSecurityRule securityRule,
        Expression<Func<IPrincipal<TIdent>, bool>> principalFilter);

    List<Dictionary<Type, IEnumerable<TIdent>>> GetPermissions(
        SecurityRule.DomainObjectSecurityRule securityRule);

    IQueryable<IPermission<TIdent>> GetPermissionQuery(SecurityRule.DomainObjectSecurityRule securityRule);
}
