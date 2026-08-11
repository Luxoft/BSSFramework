using Framework.Authorization.Generated.DTO;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
                evaluateData.Context.CurrentPrincipalSource.CurrentUser.ToFullDTO(evaluateData.MappingService));
}
