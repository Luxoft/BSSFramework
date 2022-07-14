using System;

using Framework.Configuration.BLL;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;

using Serilog.Context;

namespace SampleSystem.WebApiCore.Controllers
{
    public class ConfigSLJsonController : Framework.Configuration.WebApi.ConfigSLJsonController
    {
        public ConfigSLJsonController(IServiceEnvironment environment, IExceptionProcessor exceptionProcessor)
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
    }
}
