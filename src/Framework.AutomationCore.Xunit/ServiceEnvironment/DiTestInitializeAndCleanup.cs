using Automation.ServiceEnvironment;
using Automation.ServiceEnvironment.Services;
using Automation.Settings;
using Automation.Utils.DatabaseUtils.Interfaces;

using Bss.Testing.Xunit.Interfaces;

using Microsoft.Extensions.Options;

using SecuritySystem.Testing;

namespace Automation.Xunit.ServiceEnvironment;

public class DiTestInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    IDatabaseContext databaseContext,
    IntegrationTestTimeProvider timeProvider,
    ITestingUserAuthenticationService userAuthenticationService)
    : TestInitializeAndCleanup(settings, databaseContext, timeProvider, userAuthenticationService),
      ITestInitializeAndCleanup;
