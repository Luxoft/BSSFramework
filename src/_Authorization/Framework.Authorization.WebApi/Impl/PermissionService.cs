﻿using Framework.Authorization.Domain;
using Framework.Authorization.Environment;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ChangeDelegatePermissions))]
    public void ChangeDelegatePermissions([FromForm] ChangePermissionDelegatesModelStrictDTO changePermissionDelegatesModelStrictDTO)
    {
        if (changePermissionDelegatesModelStrictDTO == null)
            throw new ArgumentNullException(nameof(changePermissionDelegatesModelStrictDTO));

        this.Evaluate(
            DBSessionMode.Write,
            evaluateData =>
            {
                var changePermissionDelegatesModel = changePermissionDelegatesModelStrictDTO.ToDomainObject(evaluateData.MappingService);

                var securityProvider = new PrincipalSecurityProvider<Permission>(
                        evaluateData.Context.ActualPrincipalSource,
                        permission => permission.Principal)
                    .Or(evaluateData.Context.SecurityService.GetSecurityProvider<Permission>(SpecialRoleSecurityRule.Administrator));

                var bll = evaluateData.Context.Logics.PermissionFactory.Create(securityProvider);

                bll.ChangeDelegatePermissions(changePermissionDelegatesModel);
            });
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetVisualBusinessRolesByPermission))]
    public IEnumerable<BusinessRoleVisualDTO> GetVisualBusinessRolesByPermission([FromForm] PermissionIdentityDTO permission)
    {
        return new[]
               {
                   this.Evaluate(
                       DBSessionMode.Write,
                       evaluateData =>
                           evaluateData.Context.Logics.PermissionFactory
                                       .Create(SecurityRule.View)
                                       .GetById(permission.Id, true)
                                       .Role
                                       .ToVisualDTO(evaluateData.MappingService))
               };
    }
}
