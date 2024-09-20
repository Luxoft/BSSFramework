using Automation.Settings;

using Framework.DomainDriven.Jobs;
using Framework.DomainDriven.WebApiNetCore.Auth;
using Framework.DomainDriven.ScopedEvaluate;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment;

public class IntegrationTestJobRunner(
    IServiceProvider rootServiceProvider,
    IOptions<AutomationFrameworkSettings> automationFrameworkSettings,
    IApplicationDefaultUserAuthenticationServiceSettings applicationDefaultUserAuthenticationServiceSettings) : IIntegrationTestJobRunner
{
    public async Task RunJob<TJob>(Func<TJob, Task> executeAsync)
    {
        await using var scope = rootServiceProvider.CreateAsyncScope();

        var middlewareFactory = scope.ServiceProvider.GetRequiredService<IJobMiddlewareFactory>();

        var job = scope.ServiceProvider.GetRequiredService<TJob>();

        await middlewareFactory
              .Create<TJob>(automationFrameworkSettings.Value.JobRunAs ?? applicationDefaultUserAuthenticationServiceSettings.DefaultValue)
              .EvaluateAsync(async () => await executeAsync(job));
    }
}
