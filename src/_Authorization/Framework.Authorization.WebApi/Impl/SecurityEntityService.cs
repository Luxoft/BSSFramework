using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetFullSecurityEntities))]
    public IEnumerable<SecurityEntityFullDTO> GetFullSecurityEntities([FromForm] SecurityContextTypeIdentityDTO securityContextTypeIdentity)
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     var securityContextType = evaluateData.Context.Logics.SecurityContextType.GetById(securityContextTypeIdentity.Id, true);

                                                     return evaluateData.Context.ExternalSource.GetTyped(securityContextType).GetSecurityEntities().ToFullDTOList(evaluateData.MappingService);
                                                 });
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetFullSecurityEntitiesByIdents))]
    public IEnumerable<SecurityEntityFullDTO> GetFullSecurityEntitiesByIdents([FromForm] GetFullSecurityEntitiesByIdentsRequest request)
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     var securityContextType = evaluateData.Context.Logics.SecurityContextType.GetById(request.SecurityContextType.Id, true);

                                                     return evaluateData.Context.ExternalSource.GetTyped(securityContextType).GetSecurityEntitiesByIdents(request.SecurityEntities.Select(v => v.Id)).ToFullDTOList(evaluateData.MappingService);
                                                 });
    }
}
