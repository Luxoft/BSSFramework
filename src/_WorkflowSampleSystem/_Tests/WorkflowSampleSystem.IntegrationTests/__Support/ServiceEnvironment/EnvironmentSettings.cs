using System;

using Framework.Core;
using Framework.DomainDriven.ServiceModel;

namespace WorkflowSampleSystem.ServiceEnvironment
{
    public class EnvironmentSettings
    {
        public static readonly EnvironmentSettings Trace = new EnvironmentSettings(new NotificationContext(MessageSender<object>.Trace, "WorkflowSampleSystem_Sender@luxoft.com", "WorkflowSampleSystem_Reciver@luxoft.com"));

        public readonly INotificationContext NotificationContext;

        public EnvironmentSettings(INotificationContext notificationContext)
        {
            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
        }
    }
}
