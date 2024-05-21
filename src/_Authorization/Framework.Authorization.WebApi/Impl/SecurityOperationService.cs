using Framework.DomainDriven;

namespace Framework.Authorization.WebApi;

public partial class AuthSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetSecurityOperations))]
    public IEnumerable<string> GetSecurityOperations()
    {
        return this.EvaluateC(DBSessionMode.Read,
                              context => context.AvailableSecurityRoleSource
                                                .GetAvailableSecurityRoles()
                                                .GetAwaiter()
                                                .GetResult()
                                                .SelectMany(sr => sr.Information.Operations)
                                                .Distinct()
                                                .Select(op => op.Name)
                                                .ToList());
    }
}
