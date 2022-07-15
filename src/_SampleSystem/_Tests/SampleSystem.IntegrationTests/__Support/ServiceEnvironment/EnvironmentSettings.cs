using System;

using Framework.DomainDriven.ServiceModel;

namespace SampleSystem.ServiceEnvironment
{
    public class EnvironmentSettings
    {
        public static readonly EnvironmentSettings Trace = new EnvironmentSettings(new NotificationContext("SampleSystem_Sender@luxoft.com"));


        public EnvironmentSettings(INotificationContext notificationContext)
        {
            this.NotificationContext = notificationContext ?? throw new ArgumentNullException(nameof(notificationContext));
        }

        public INotificationContext NotificationContext { get; }
    }
}
