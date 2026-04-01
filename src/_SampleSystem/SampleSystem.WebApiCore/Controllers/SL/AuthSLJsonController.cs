using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.BLL;
using Framework.BLL.DTOMapping.Domain;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

using SecuritySystem;

namespace SampleSystem.WebApiCore.Controllers;

public class AuthMainController : Framework.Authorization.WebApi.AuthMainController
{
    [HttpPost]
    public PermissionIdentityDTO SavePermission(SavePermissionAutoRequest savePermissionAutoRequest)
    {
        return this.Evaluate(DBSessionMode.Write, evaluateData =>
                                                  {
                                                      var principalIdent = savePermissionAutoRequest.PrincipalIdent;
                                                      var permissionDTO = savePermissionAutoRequest.PermissionDTO;

                                                      var principalBLL = evaluateData.Context.Logics.PrincipalFactory.Create(SecurityRule.Edit);
                                                      var permissionBLL = evaluateData.Context.Logics.PermissionFactory.Create(SecurityRule.Edit);

                                                      var principal = principalBLL.GetById(principalIdent.Id, true);

                                                      var permission = permissionBLL.GetById(permissionDTO.Id, IdCheckMode.SkipEmpty) ?? new Permission(principal);

                                                      permissionDTO.MapToDomainObject(evaluateData.MappingService, permission);

                                                      permissionBLL.Save(permission);

                                                      return permission.ToIdentityDTO();
                                                  });
    }



    [System.Runtime.Serialization.DataContractAttribute]
    [AutoRequest]
    public class SavePermissionAutoRequest
    {
        public SavePermissionAutoRequest()
        {

        }

        public SavePermissionAutoRequest(PrincipalIdentityDTO principalIdent, PermissionStrictDTO permissionDTO)
        {
            this.PrincipalIdent = principalIdent;
            this.PermissionDTO = permissionDTO;
        }

        [System.Runtime.Serialization.DataMemberAttribute]
        [AutoRequestProperty(OrderIndex = 0)]
        public PrincipalIdentityDTO PrincipalIdent { get; set; }

        [System.Runtime.Serialization.DataMemberAttribute]
        [AutoRequestProperty(OrderIndex = 1)]
        public PermissionStrictDTO PermissionDTO { get; set; }
    }
}
