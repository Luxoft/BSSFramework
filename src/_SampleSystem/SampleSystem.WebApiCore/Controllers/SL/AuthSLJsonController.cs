using System;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers
{
    public class AuthSLJsonController : Framework.Authorization.WebApi.AuthSLJsonController
    {
        public AuthSLJsonController()
            
        {
        }


        [HttpPost(nameof(SavePermission))]
        public PermissionIdentityDTO SavePermission(SavePermissionAutoRequest savePermissionAutoRequest)
        {
            return this.Evaluate(DBSessionMode.Write, evaluateData =>
                                                      {
                                                          var principalIdent = savePermissionAutoRequest.principalIdent;
                                                          var permissionDTO = savePermissionAutoRequest.permissionDTO;

                                                          var principalBLL = evaluateData.Context.Logics.PrincipalFactory.Create(BLLSecurityMode.Edit);
                                                          var permissionBLL = evaluateData.Context.Logics.PermissionFactory.Create(BLLSecurityMode.Edit);

                                                          var principal = principalBLL.GetById(principalIdent.Id, true);

                                                          var permission = permissionBLL.GetById(permissionDTO.Id, IdCheckMode.SkipEmpty) ?? new Permission(principal);

                                                          permissionDTO.MapToDomainObject(evaluateData.MappingService, permission);

                                                          permissionBLL.Save(permission);

                                                          return permission.ToIdentityDTO();
                                                      });
        }



        [System.Runtime.Serialization.DataContractAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
        public class SavePermissionAutoRequest
        {
            public SavePermissionAutoRequest()
            {

            }

            public SavePermissionAutoRequest(PrincipalIdentityDTO principalIdent, PermissionStrictDTO permissionDTO)
            {
                this.principalIdent = principalIdent;
                this.permissionDTO = permissionDTO;
            }

            [System.Runtime.Serialization.DataMemberAttribute()]
            [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex = 0)]
            public PrincipalIdentityDTO principalIdent;

            [System.Runtime.Serialization.DataMemberAttribute()]
            [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex = 1)]
            public PermissionStrictDTO permissionDTO;
        }
    }
}
