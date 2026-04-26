using Bss.Testing.Xunit.Interfaces;

using Framework.AutomationCore.ServiceEnvironment.Services;
using Framework.AutomationCore.Settings;
using Framework.AutomationCore.Utils.DatabaseUtils.Interfaces;

using Microsoft.Extensions.Options;

using Anch.SecuritySystem.Testing;

namespace Framework.AutomationCore.Xunit.ServiceEnvironment;

public class DiTestInitializeAndCleanup(
    IOptions<AutomationFrameworkSettings> settings,
    IDatabaseContext databaseContext,
    IntegrationTestTimeProvider timeProvider,
    RootImpersonateServiceState rootImpersonateServiceState)
    : TestInitializeAndCleanup(settings, databaseContext, timeProvider, rootImpersonateServiceState),
      ITestInitializeAndCleanup;
