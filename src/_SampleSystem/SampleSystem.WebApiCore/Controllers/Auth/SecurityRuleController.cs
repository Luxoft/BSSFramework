using Framework.SecuritySystem.AvailableSecurity;

using Microsoft.AspNetCore.Mvc;

namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class SecurityRuleController(IAvailableClientSecurityRuleSource availableClientSecurityRuleSource) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<string>> GetSecurityRules(CancellationToken cancellationToken)
    {
        var clientSecurityRules = await availableClientSecurityRuleSource.GetAvailableSecurityRules(cancellationToken);

        return clientSecurityRules.Select(sr => sr.Name);
    }
}
