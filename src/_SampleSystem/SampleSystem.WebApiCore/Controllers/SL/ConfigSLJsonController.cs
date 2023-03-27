using Framework.Configuration.BLL;
using Framework.DomainDriven;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers;

public class ConfigSLJsonController : Framework.Configuration.WebApi.ConfigSLJsonController
{
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
