using Framework.Core;
using Framework.DomainDriven;

namespace Authorization.WebApi.Controllers;

public partial class OperationController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSecurityOperations))]
    public IEnumerable<string> GetSecurityOperations()
    {
        return this.EvaluateC(DBSessionMode.Read, context => context.AvailableSecurityRoleSource.GetAvailableSecurityRole().GetAwaiter().GetResult().ToList(op => op.Name));
    }
}
