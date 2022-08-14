using Framework.Authorization.Generated.DTO;
using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Authorization.WebApi
{
    public partial class AuthSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(RunAsUser))]
        public void RunAsUser([FromForm] PrincipalIdentityDTO principal)
        {
            this.EvaluateC(DBSessionMode.Write, context =>
            {
                var runAsPrincipal = context.Logics.Principal.GetById(principal.Id, true);

                context.RunAsManager.StartRunAsUser(runAsPrincipal.Name);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(FinishRunAsUser))]
        public void FinishRunAsUser()
        {
            this.Evaluate(DBSessionMode.Write, evaluateData => evaluateData.Context.RunAsManager.FinishRunAsUser());
        }
    }
}
