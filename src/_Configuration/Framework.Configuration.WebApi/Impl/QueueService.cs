using Framework.Core;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Mvc;

namespace Framework.Configuration.WebApi;

public partial class ConfigSLJsonController
{
    [HttpPost]
    public int ProcessModifications(int limit)
    {
        var result = this.EvaluateC(DBSessionMode.Write, context =>
                                                         {
                                                             context.Authorization.SecuritySystem.CheckAccess(SecurityRole.SystemIntegration);
                                                             return context.Logics.DomainObjectModification.Process(limit == default(int) ? 1000 : limit);
                                                         });

        return result.Match(v => v, ex => throw ex);
    }

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetEventQueueProcessingState() =>
        this.Evaluate(DBSessionMode.Read, evaluateData =>
                                          {
                                              evaluateData.Context.Authorization.SecuritySystem.CheckAccess(SecurityRole.SystemIntegration);

                                              return evaluateData.Context.Logics.DomainObjectEvent.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                          });

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetModificationQueueProcessingState() =>
        this.Evaluate(DBSessionMode.Read, evaluateData =>
                                          {
                                              evaluateData.Context.Authorization.SecuritySystem.CheckAccess(SecurityRole.SystemIntegration);

                                              return evaluateData.Context.Logics.DomainObjectModification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                          });

    [HttpPost]
    public QueueProcessingStateSimpleDTO GetNotificationQueueProcessingState() =>
        this.Evaluate(DBSessionMode.Read, evaluateData =>
                                          {
                                              evaluateData.Context.Authorization.SecuritySystem.CheckAccess(SecurityRole.SystemIntegration);

                                              return evaluateData.Context.Logics.DomainObjectNotification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                          });
}
