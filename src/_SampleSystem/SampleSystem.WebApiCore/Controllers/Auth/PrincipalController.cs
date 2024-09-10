using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                         evaluateData.Context.Authorization.CurrentPrincipalSource.CurrentPrincipal.ToFullDTO(evaluateData.MappingService));
    }
}
