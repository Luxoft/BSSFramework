using Framework.Authorization.Generated.DTO;
using Framework.Database;

using SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthMainController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal() => this.Evaluate(DBSessionMode.Read, evaluateData => evaluateData.Context.CurrentPrincipalSource.CurrentUser.ToFullDTO(evaluateData.MappingService));

    [HttpPost]
    public IEnumerable<PrincipalVisualDTO> GetVisualPrincipalsWithoutSecurity() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
                evaluateData.Context.SecuritySystem.HasAccessAsync(DomainSecurityRule.AnyRole, this.HttpContext.RequestAborted).GetAwaiter().GetResult()
                    ? evaluateData.Context.Logics.Principal.GetFullList().ToVisualDTOList(evaluateData.MappingService)
                    : Enumerable.Empty<PrincipalVisualDTO>());
}
