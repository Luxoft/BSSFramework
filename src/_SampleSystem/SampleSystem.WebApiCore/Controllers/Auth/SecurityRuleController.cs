using Anch.SecuritySystem.AvailableSecurity;

using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Authorization.WebApi.Controllers;

[ApiController]
[Route("authApi/[controller]/[action]")]
public class SecurityRuleController(IAvailableClientSecurityRuleSource availableClientSecurityRuleSource) : ControllerBase
{
    [HttpGet]
    public async Task<List<string>> GetSecurityRules(CancellationToken ct) =>
        await availableClientSecurityRuleSource.GetAvailableSecurityRules()
                                               .Select(sr => sr.Name)
                                               .ToListAsync(ct);
}

