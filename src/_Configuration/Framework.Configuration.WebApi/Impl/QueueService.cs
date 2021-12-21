using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DTO;

using Serilog.Context;

namespace Framework.Configuration.WebApi
{
    public partial class ConfigSLJsonController
    {
        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(ProcessModifications))]
        public int ProcessModifications(int limit)
        {
            var result = this.EvaluateC(DBSessionMode.Write, context =>
            {
                using (LogContext.PushProperty("Method", nameof(ConfigurationSecurityOperation.ProcessModifications)))
                {
                    context.Authorization.CheckAccess(ConfigurationSecurityOperation.ProcessModifications);

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
                evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.QueueMonitoring);

                return evaluateData.Context.Logics.DomainObjectEvent.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetModificationQueueProcessingState))]
        public QueueProcessingStateSimpleDTO GetModificationQueueProcessingState()
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.QueueMonitoring);

                return evaluateData.Context.Logics.DomainObjectModification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost(nameof(GetNotificationQueueProcessingState))]
        public QueueProcessingStateSimpleDTO GetNotificationQueueProcessingState()
        {
            return this.Evaluate(DBSessionMode.Read, evaluateData =>
            {
                evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.QueueMonitoring);

                return evaluateData.Context.Logics.DomainObjectNotification.GetProcessingState().ToSimpleDTO(evaluateData.MappingService);
            });
        }
    }
}
