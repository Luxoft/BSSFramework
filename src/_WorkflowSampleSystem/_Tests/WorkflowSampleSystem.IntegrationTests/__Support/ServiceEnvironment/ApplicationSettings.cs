using System;
using System.Configuration;
using System.Net.Mail;

using Framework.Core;
using Framework.DomainDriven.ServiceModel;
using Framework.Notification.DTO;

using JetBrains.Annotations;

namespace WorkflowSampleSystem.ServiceEnvironment
{
    public class ApplicationSettings
    {
        public readonly string MsmqServerName;

        public readonly string IntegrationEventQueueName;

        public readonly string NotificationQueueName;

        public readonly string NotificationFromAddress;

        public readonly string NotificationFromAddressDisplayName;

        public readonly string ExceptionNotificationEmails;

        public readonly bool NotificationEnabled;

        private static readonly Lazy<ApplicationSettings> LazyConfiguration = LazyHelper.Create(() =>
        {
            var msmqServerName = ConfigurationManager.AppSettings["MsmqServer"];
            var integrationEventQueueName = ConfigurationManager.AppSettings["IntegrationEventQueueName"];
            var notificationQueueName = ConfigurationManager.AppSettings["NotificationsQueueName"];
            var notificationFromAddress = ConfigurationManager.AppSettings["notificationFromAddress"];
            var notificationFromAddressDisplayName = ConfigurationManager.AppSettings["notificationFromAddressDisplayName"];
            var exceptionNotificationEmails = ConfigurationManager.AppSettings["ExceptionNotificationEmails"];

            var notificationEnabled = bool.Parse(ConfigurationManager.AppSettings["NotificationsEnable"]);
            var isTestRunMode = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["TestRunMode"]);

            return new ApplicationSettings(
                msmqServerName,
                integrationEventQueueName,
                notificationQueueName,
                notificationFromAddress,
                notificationFromAddressDisplayName,
                exceptionNotificationEmails,
                notificationEnabled,
                isTestRunMode);
        });

        public ApplicationSettings(
            [NotNull] string msmqServerName,
            [NotNull] string integrationEventQueueName,
            [NotNull] string notificationQueueName,
            [NotNull] string notificationFromAddress,
            [NotNull] string notificationFromAddressDisplayName,
            [NotNull] string exceptionNotificationEmails,
            bool notificationEnabled,
            bool isTestRunMode)
        {
            if (msmqServerName == null) throw new ArgumentNullException(nameof(msmqServerName));
            if (integrationEventQueueName == null) throw new ArgumentNullException(nameof(integrationEventQueueName));
            if (notificationQueueName == null) throw new ArgumentNullException(nameof(notificationQueueName));
            if (notificationFromAddress == null) throw new ArgumentNullException(nameof(notificationFromAddress));
            if (notificationFromAddressDisplayName == null) throw new ArgumentNullException(nameof(notificationFromAddressDisplayName));
            if (exceptionNotificationEmails == null) throw new ArgumentNullException(nameof(exceptionNotificationEmails));

            this.MsmqServerName = msmqServerName;
            this.IntegrationEventQueueName = integrationEventQueueName;
            this.NotificationQueueName = notificationQueueName;
            this.NotificationFromAddress = notificationFromAddress;
            this.NotificationFromAddressDisplayName = notificationFromAddressDisplayName;
            this.ExceptionNotificationEmails = exceptionNotificationEmails;
            this.NotificationEnabled = notificationEnabled;
        }

        public static ApplicationSettings Configuration => LazyConfiguration.Value;

        public EnvironmentSettings ToEnvironmentSettings()
        {
            var mailAddress = new MailAddress(this.NotificationFromAddress, this.NotificationFromAddressDisplayName);

            var notificationContext = new NotificationContext(MessageSender<NotificationEventDTO>.Trace, mailAddress, this.ExceptionNotificationEmails.Split(','));

            return new EnvironmentSettings(
                notificationContext);
        }
    }
}
