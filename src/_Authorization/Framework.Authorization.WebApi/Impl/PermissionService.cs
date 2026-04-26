using Framework.Authorization.Generated.DTO;
using Framework.Database;

using Anch.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Framework.Authorization.WebApi;

public partial class AuthMainController
{
    [HttpPost]
    public void ChangeDelegatePermissions([FromForm] ChangePermissionDelegatesModelStrictDTO changePermissionDelegatesModelStrictDTO)
    {
        if (changePermissionDelegatesModelStrictDTO == null)
            throw new ArgumentNullException(nameof(changePermissionDelegatesModelStrictDTO));

        this.Evaluate(
            DBSessionMode.Write,
            evaluateData =>
            {
                var changePermissionDelegatesModel = changePermissionDelegatesModelStrictDTO.ToDomainObject(evaluateData.MappingService);

                var bll = evaluateData.Context.Logics.PermissionFactory.Create(SecurityRule.Edit);

                bll.ChangeDelegatePermissions(changePermissionDelegatesModel);
            });
    }

    [HttpPost]
    public IEnumerable<BusinessRoleVisualDTO> GetVisualBusinessRolesByPermission([FromForm] PermissionIdentityDTO permission) =>
    [
        this.Evaluate(
                DBSessionMode.Write,
                evaluateData =>
                    evaluateData.Context.Logics.PermissionFactory
                                .Create(SecurityRule.View)
                                .GetById(permission.Id, true)
                                .Role
                                .ToVisualDTO(evaluateData.MappingService))
    ];
}
