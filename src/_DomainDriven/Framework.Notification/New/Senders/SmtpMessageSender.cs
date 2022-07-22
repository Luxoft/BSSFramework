using System;
using System.Configuration;
using System.Net.Mail;

using Framework.Core;

namespace Framework.Notification.New
{
    /// <summary>
    /// SMTP sender implementation
    /// </summary>
    public class SmtpMessageSender : IMessageSender<Message>
    {
        /// <summary>
        /// Gets SmtpMessageSender instance using SmtpServer option from configuration file.
        /// </summary>
        public static readonly SmtpMessageSender Configuration = ConfigurationManager.AppSettings["SmtpServer"].Pipe(
            host => new SmtpMessageSender(
                () =>
                    {
                        var smtpClient = new SmtpClient();

                        if (!string.IsNullOrWhiteSpace(host))
                        {
                            smtpClient.Host = host;
                        }

                        return smtpClient;
                    }));

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
}
