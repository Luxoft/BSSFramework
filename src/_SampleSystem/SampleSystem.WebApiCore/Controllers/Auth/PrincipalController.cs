using Framework.Authorization.Generated.DTO;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal() =>
        this.Evaluate(DBSessionMode.Read, evaluateData =>
                          evaluateData.Context.Authorization.CurrentPrincipalSource.CurrentUser.ToFullDTO(evaluateData.MappingService));
}
