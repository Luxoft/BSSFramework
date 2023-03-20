using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

namespace Authorization.WebApi.Controllers;

public partial class PrincipalController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetCurrentPrincipal))]
    public PrincipalFullDTO GetCurrentPrincipal()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                         evaluateData.Context.Logics.Principal.GetCurrent().ToFullDTO(evaluateData.MappingService));
    }
}
