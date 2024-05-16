using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetCurrentPrincipal))]
    public PrincipalFullDTO GetCurrentPrincipal()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                         evaluateData.Context.Authorization.CurrentPrincipal.ToFullDTO(evaluateData.MappingService));
    }
}
