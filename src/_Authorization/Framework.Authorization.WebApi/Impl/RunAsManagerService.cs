using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [HttpPost]
    public void RunAsUser([FromForm] PrincipalIdentityDTO principal)
    {
        this.EvaluateC(DBSessionMode.Write, context =>
                                            {
                                                var runAsPrincipal = context.Logics.Principal.GetById(principal.Id, true);

                                                context.RunAsManager.StartRunAsUserAsync(runAsPrincipal.Name).GetAwaiter().GetResult();
                                            });
    }

    [HttpPost]
    public void FinishRunAsUser()
    {
        this.Evaluate(DBSessionMode.Write, evaluateData => evaluateData.Context.RunAsManager.FinishRunAsUserAsync().GetAwaiter().GetResult());
    }
}
