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
                                                     return evaluateData.Context.ExternalSource.GetTyped(securityContextTypeIdentity.Id).GetSecurityEntities().ToFullDTOList(evaluateData.MappingService);
                                                 });
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetFullSecurityEntitiesByIdents))]
    public IEnumerable<SecurityEntityFullDTO> GetFullSecurityEntitiesByIdents([FromForm] GetFullSecurityEntitiesByIdentsRequest request)
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     return evaluateData.Context.ExternalSource.GetTyped(request.SecurityContextType.Id).GetSecurityEntitiesByIdents(request.SecurityEntities.Select(v => v.Id)).ToFullDTOList(evaluateData.MappingService);
                                                 });
    }
}
