using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost]
    public IEnumerable<SecurityContextData> GetFullSecurityEntities([FromForm] SecurityContextTypeIdentityDTO securityContextTypeIdentity)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context
                                   .SecurityContextStorage
                                   .GetTyped(securityContextTypeIdentity.Id)
                                   .GetSecurityContexts()
                                   .ToList();
            });
    }

    [HttpPost]
    public IEnumerable<SecurityContextData> GetFullSecurityEntitiesByIdents([FromForm] GetFullSecurityEntitiesByIdentsRequest request)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context.SecurityContextStorage.GetTyped(request.SecurityContextType.Id)
                                   .GetSecurityContextsByIdents(request.SecurityEntities.Select(v => v.Id))
                                   .ToList();
            });
    }
}
