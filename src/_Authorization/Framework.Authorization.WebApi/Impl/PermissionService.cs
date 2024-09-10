using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
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
