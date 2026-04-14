using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [HttpPost]
    public PrincipalFullDTO GetCurrentPrincipal() =>
        this.Evaluate(DBSessionMode.Read, evaluateData =>
                          LambdaHelper.ToFullDTO((Principal)evaluateData.Context.Authorization.CurrentPrincipalSource.CurrentUser, evaluateData.MappingService));
}
