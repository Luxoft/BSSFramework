using System;
using System.Collections.Generic;

using Framework.Configuration.BLL;
using Framework.Configuration.Generated.DTO;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;

using Serilog.Context;

namespace SampleSystem.WebApiCore.Controllers
{
    public class ConfigSLJsonController : Framework.Configuration.WebApi.ConfigSLJsonController
    {
        public ConfigSLJsonController(IServiceEnvironment<IConfigurationBLLContext> environment, IExceptionProcessor exceptionProcessor)
            : base(environment, exceptionProcessor)
        {
        }

        [HttpPost(nameof(SaveSendedNotification))]
        public void SaveSendedNotification(NotificationEventDTO notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            this.EvaluateC(DBSessionMode.Write, context =>
            {
                var bll = new SentMessageBLL(context);

                bll.Save(notification.ToSentMessage());
            });
        }



        /// <inheritdoc />
        [HttpPost(nameof(GetPulseJobs))]
        public List<RunRegularJobModelStrictDTO> GetPulseJobs()
        {
            return this.Evaluate(
                DBSessionMode.Write,
                evaluateData =>
                {
                    using (LogContext.PushProperty("Method", "GetPulseJobs"))
                    {
                        evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.SystemIntegration);

                        var list = evaluateData.Context.Logics.RegularJob.GetPulseJobs()
                                               .ToRichDTOList(evaluateData.MappingService)
                                               .ToList(dto => dto.ToStrict());

                        return list;
                    }
                });
        }

        [HttpPost(nameof(PulseJobs))]
        public void PulseJobs()
        {
            this.EvaluateC(
                           DBSessionMode.Write,
                           context =>
                           {
                               using (LogContext.PushProperty("Method", "PulseJobs"))
                               {
                                   context.Authorization.CheckAccess(ConfigurationSecurityOperation.SystemIntegration);

                                   context.Logics.RegularJob.Pulse();
                               }
                           });
        }

        [HttpPost(nameof(RunRegularJob))]
        public void RunRegularJob(RunRegularJobModelStrictDTO runRegularJobModelStrictDto)
        {
            this.Evaluate(
                          DBSessionMode.Read,
                          evaluateData =>
                          {
                              using (LogContext.PushProperty("Method", "RunRegularJob"))
                              {
                                  evaluateData.Context.Authorization.CheckAccess(ConfigurationSecurityOperation.SystemIntegration);

                                  var model = runRegularJobModelStrictDto.ToDomainObject(evaluateData.MappingService);

                                  evaluateData.Context.Logics.RegularJob.RunRegularJobInNewSession(model.RegularJob, model.Mode, false);
                              }
                          });
        }
    }
}
