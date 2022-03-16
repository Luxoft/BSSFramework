using System;

using Framework.Core;
using Framework.DomainDriven.ServiceModel;

namespace AttachmentsSampleSystem.ServiceEnvironment
{
    public class EnvironmentSettings
    {
        public static readonly EnvironmentSettings Trace = new EnvironmentSettings(new NotificationContext(MessageSender<object>.Trace, "AttachmentsSampleSystem_Sender@luxoft.com", "AttachmentsSampleSystem_Reciver@luxoft.com"));

        public readonly INotificationContext NotificationContext;

        public EnvironmentSettings(INotificationContext notificationContext)
        {
            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
        }
    }
}
