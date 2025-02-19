using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost]
    public IEnumerable<SecurityContextData> GetFullSecurityContexts([FromForm] SecurityContextTypeIdentityDTO securityContextTypeIdentity)
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
    public IEnumerable<SecurityContextData> GetFullSecurityContextsByIdents([FromForm] GetFullSecurityContextsByIdentsRequest request)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context.SecurityContextStorage.GetTyped(request.SecurityContextType.Id)
                                   .GetSecurityContextsByIdents(request.SecurityContexts.Select(v => v.Id))
                                   .ToList();
            });
    }
}
