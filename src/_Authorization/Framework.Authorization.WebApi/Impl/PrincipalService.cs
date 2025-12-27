using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData => evaluateData.Context.CurrentPrincipalSource.CurrentUser.ToFullDTO(evaluateData.MappingService));
    }

    [HttpPost]
    public IEnumerable<PrincipalVisualDTO> GetVisualPrincipalsWithoutSecurity()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                         evaluateData.Context.SecuritySystem.HasAccess(DomainSecurityRule.AnyRole)
                                                                 ? evaluateData.Context.Logics.Principal.GetFullList().ToVisualDTOList(evaluateData.MappingService)
                                                                 : Enumerable.Empty<PrincipalVisualDTO>());
    }
}
