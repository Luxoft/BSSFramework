using Microsoft.AspNetCore.Mvc;

using SecuritySystem.AvailableSecurity;

namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class SecurityRuleController(IAvailableClientSecurityRuleSource availableClientSecurityRuleSource) : ControllerBase
{
    [HttpGet]
    public async Task<List<string>> GetSecurityRules(CancellationToken cancellationToken) =>
        await availableClientSecurityRuleSource.GetAvailableSecurityRules()
                                               .Select(sr => sr.Name)
                                               .ToListAsync(cancellationToken);
}
