using Anch.SecuritySystem;

using Framework.Application.Jobs;
using Framework.AutomationCore.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.AutomationCore.Extensions;

public static class ServiceProviderExtensions
{
    extension(IServiceProvider rootServiceProvider)
    {
        public async Task RunJob<TJob>(CancellationToken cancellationToken = default)
            where TJob : IJob =>
            await rootServiceProvider.GetRequiredService<IJobServiceEvaluatorFactory>().RunJob<TJob>(cancellationToken);

        public ControllerEvaluator<TController> GetDefaultControllerEvaluator<TController>(UserCredential? userCredential = null)
            where TController : ControllerBase
        {
            var controllerEvaluator = rootServiceProvider.GetRequiredService<ControllerEvaluator<TController>>();

            return userCredential == null ? controllerEvaluator : controllerEvaluator.WithImpersonate(userCredential);
        }
    }
}

