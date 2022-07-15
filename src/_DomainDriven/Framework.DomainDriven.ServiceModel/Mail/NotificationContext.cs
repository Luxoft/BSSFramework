using System;
using System.Net.Mail;

namespace Framework.DomainDriven.ServiceModel
{
    public class NotificationContext : INotificationContext
    {
        public NotificationContext(string systemSender)
            : this(new MailAddress(systemSender, systemSender))
        {
        }

        public NotificationContext(MailAddress systemSender)
        {
            if (systemSender == null) throw new ArgumentNullException(nameof(systemSender));
            if (string.IsNullOrWhiteSpace(systemSender.DisplayName)) throw new System.ArgumentException("Not initialize sender name in NotificationContext", nameof(systemSender));

            this.SystemSender = systemSender;
        }

        public MailAddress SystemSender { get; }
    }
}
