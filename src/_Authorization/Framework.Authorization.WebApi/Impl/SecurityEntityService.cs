using CommonFramework;

using Framework.Authorization.Generated.DTO;
using Framework.Database;

using SecuritySystem.ExternalSystem.SecurityContextStorage;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthMainController
{
    [HttpPost]
    public IEnumerable<SecurityContextData<Guid>> GetFullSecurityEntities(
        [FromForm] SecurityContextTypeIdentityDTO securityContextTypeIdentity) =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context
                                   .SecurityContextStorage
                                   .GetTyped(
                                       evaluateData.Context.SecurityContextInfoSource.GetSecurityContextInfo(securityContextTypeIdentity.Id)
                                                   .Type)
                                   .Pipe(v => (ITypedSecurityContextStorage<Guid>)v)
                                   .GetSecurityContexts()
                                   .ToList();
            });

    [HttpPost]
    public IEnumerable<SecurityContextData<Guid>> GetFullSecurityEntitiesByIdents([FromForm] GetFullSecurityEntitiesByIdentsRequest request) =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                return evaluateData.Context
                                   .SecurityContextStorage
                                   .GetTyped(
                                       evaluateData.Context.SecurityContextInfoSource.GetSecurityContextInfo(request.SecurityContextType.Id)
                                                   .Type)
                                   .Pipe(v => (ITypedSecurityContextStorage<Guid>)v)
                                   .GetSecurityContextsByIdents(request.SecurityEntities.Select(v => v.Id))
                                   .ToList();
            });
}
