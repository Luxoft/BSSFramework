using Framework.Core;
using Framework.Configuration.Generated.DTO;
using Framework.DomainDriven;
using Framework.SecuritySystem;

using Serilog.Context;

namespace Framework.Configuration.WebApi;

public partial class ConfigSLJsonController
{
    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ProcessModifications))]
    public int ProcessModifications(int limit)
    {
        var result = this.EvaluateC(DBSessionMode.Write, context =>
                                                         {
                                                             using (LogContext.PushProperty("Method", nameof(this.ProcessModifications)))
                                                             {
                                                                 context.Authorization.AuthorizationSystem.CheckAccess(SpecialRoleSecurityRule.SystemIntegration);

                                                                 return context.Logics.DomainObjectModification.Process(limit == default(int) ? 1000 : limit);
                                                             }
                                                         });

        return result.Match(v => v, ex => throw ex);
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetEventQueueProcessingState))]
    public QueueProcessingStateSimpleDTO GetEventQueueProcessingState()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     evaluateData.Context.Authorization.AuthorizationSystem.CheckAccess(SpecialRoleSecurityRule.SystemIntegration);

                                                     return evaluateData.Context.Logics.DomainObjectEvent.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                                 });
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetModificationQueueProcessingState))]
    public QueueProcessingStateSimpleDTO GetModificationQueueProcessingState()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     evaluateData.Context.Authorization.AuthorizationSystem.CheckAccess(SpecialRoleSecurityRule.SystemIntegration);

                                                     return evaluateData.Context.Logics.DomainObjectModification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                                 });
    }

    [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetNotificationQueueProcessingState))]
    public QueueProcessingStateSimpleDTO GetNotificationQueueProcessingState()
    {
        return this.Evaluate(DBSessionMode.Read, evaluateData =>
                                                 {
                                                     evaluateData.Context.Authorization.AuthorizationSystem.CheckAccess(SpecialRoleSecurityRule.SystemIntegration);

                                                     return evaluateData.Context.Logics.DomainObjectNotification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
                                                 });
    }
}
