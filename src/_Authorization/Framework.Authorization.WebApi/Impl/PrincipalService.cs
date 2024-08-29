using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetCurrentPrincipal))]
    public PrincipalFullDTO GetCurrentPrincipal()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData => evaluateData.Context.CurrentPrincipalSource.CurrentPrincipal.ToFullDTO(evaluateData.MappingService));
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetVisualPrincipalsWithoutSecurity))]
    public IEnumerable<PrincipalVisualDTO> GetVisualPrincipalsWithoutSecurity()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                         evaluateData.Context.AvailablePermissionSource.GetAvailablePermissionsQueryable().Any()
                                                                 ? evaluateData.Context.Logics.Principal.GetFullList().ToVisualDTOList(evaluateData.MappingService)
                                                                 : Enumerable.Empty<PrincipalVisualDTO>());
    }
}
