using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.DomainDriven.ApplicationCore.ExternalSource;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost(nameof(GetFullSecurityEntities))]
    public IEnumerable<SecurityEntity> GetFullSecurityEntities([FromForm] SecurityContextTypeIdentityDTO securityContextTypeIdentity)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context
                                   .ExternalSource
                                   .GetTyped(securityContextTypeIdentity.Id)
                                   .GetSecurityEntities()
                                   .ToList();
            });
    }

    [HttpPost(nameof(GetFullSecurityEntitiesByIdents))]
    public IEnumerable<SecurityEntity> GetFullSecurityEntitiesByIdents([FromForm] GetFullSecurityEntitiesByIdentsRequest request)
    {
        return this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context.ExternalSource.GetTyped(request.SecurityContextType.Id)
                                   .GetSecurityEntitiesByIdents(request.SecurityEntities.Select(v => v.Id))
                                   .ToList();
            });
    }
}
