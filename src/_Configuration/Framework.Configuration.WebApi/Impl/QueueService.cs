using Anch.SecuritySystem;

using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.Database;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace Framework.Configuration.WebApi;

public partial class ConfigMainController
{
    [HttpPost]
    public async Task<int> ProcessModifications(int limit, CancellationToken ct)
    {
        var context = this.HttpContext.RequestServices.GetRequiredService<IConfigurationBLLContext>();

        await context.Authorization.SecuritySystem.CheckAccessAsync(SecurityRole.SystemIntegration, ct);

        var result = await context.Logics.DomainObjectModification.ProcessAsync(limit == 0 ? 1000 : limit, ct);

        return result.Match(v => v, ex => throw ex);
    }

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetEventQueueProcessingState() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                evaluateData.Context.Authorization.SecurityService.CheckAccess(SecurityRole.SystemIntegration);

                return evaluateData.Context.Logics.DomainObjectEvent.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetModificationQueueProcessingState() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                evaluateData.Context.Authorization.SecurityService.CheckAccess(SecurityRole.SystemIntegration);

                return evaluateData.Context.Logics.DomainObjectModification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetNotificationQueueProcessingState() =>
        this.Evaluate(
            DBSessionMode.Read,
            evaluateData =>
            {
                evaluateData.Context.Authorization.SecurityService.CheckAccess(SecurityRole.SystemIntegration);

                return evaluateData.Context.Logics.DomainObjectNotification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });
}
