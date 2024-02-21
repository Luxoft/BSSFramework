using Automation.Settings;

using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment.Services;

public class DiIntegrationTestUserAuthenticationService : IntegrationTestUserAuthenticationService
{
    public DiIntegrationTestUserAuthenticationService(IOptions<AutomationFrameworkSettings> settings)
        : base(settings.Value.IntegrationTestUserName)
    {
    }
}
