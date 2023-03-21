using System;
using System.Configuration;
using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.New;

/// <summary>
/// SMTP sender implementation
/// </summary>
public class SmtpMessageSender : IMessageSender<Message>
{
    private readonly Func<SmtpClient> getSmtpClient;

    /// <summary>
    /// Initializes new sender instance
    /// </summary>
    /// <param name="getSmtpClient">Function that gets SMTP client</param>
    public SmtpMessageSender(Func<SmtpClient> getSmtpClient)
    {
        if (getSmtpClient == null)
        {
            throw new ArgumentNullException(nameof(getSmtpClient));
        }

        this.getSmtpClient = getSmtpClient;
    }

    /// <inheritdoc />
    public void Send(Message message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        var client = this.getSmtpClient();

        var mailMessage = message.ToMailMessage();

        client.Send(mailMessage);
    }
}
