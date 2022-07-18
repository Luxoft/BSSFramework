using System;
using System.Net.Mail;

namespace Framework.Configuration.BLL.Notification;

public class DefaultMailSenderContainer : IDefaultMailSenderContainer
{
    public DefaultMailSenderContainer(string defaultSender)
            : this(new MailAddress(defaultSender, defaultSender))
    {
    }

    public DefaultMailSenderContainer(MailAddress defaultSender)
    {
        if (defaultSender == null) throw new ArgumentNullException(nameof(defaultSender));
        if (string.IsNullOrWhiteSpace(defaultSender.DisplayName)) throw new System.ArgumentException("Not initialize sender name in NotificationContext", nameof(defaultSender));

        this.DefaultSender = defaultSender;
    }

    public MailAddress DefaultSender { get; }
}
