using Framework.Configuration.BLL;
using Framework.Database;
using Framework.Notification.DTO;

using Microsoft.AspNetCore.Mvc;

namespace SampleSystem.WebApiCore.Controllers;

public class ConfigMainController : Framework.Configuration.WebApi.ConfigMainController
{
    [HttpPost]
    public void SaveSendedNotification(NotificationEventDTO notification)
    {
        if (notification == null)
        {
            throw new ArgumentNullException(nameof(notification));
        }

        this.EvaluateC(
            DBSessionMode.Write,
            context =>
            {
                context.Logics.SentMessage.Save(notification.ToSentMessage());
            });
    }
}
