using System;
using System.Configuration;
using System.Net.Mail;
using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Notification.DTO;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Framework.NotificationCore.Senders
{
    public class SmtpMessageSender : IMessageSender<NotificationEventDTO>
    {
        private readonly IConfigurationBLLContext context;

        private readonly Lazy<SmtpSettings> settings;

        private readonly Lazy<IRewriteReceiversService> rewriteReceiversService;

        private readonly Lazy<ISmtpMessageSender> sender;

        public SmtpMessageSender(
            Lazy<SmtpSettings> settings,
            Lazy<IRewriteReceiversService> rewriteReceiversService,
            IConfigurationBLLContext context)
        {
            this.context = context;

            this.settings = settings;

            this.rewriteReceiversService = rewriteReceiversService;

            this.sender = LazyHelper.Create(() => this.GetSender());
        }

        private ISmtpMessageSender GetSender()
        {
            return this.IsProduction()
                    ? new ProdSmtpMessageSender(this.settings.Value, this.rewriteReceiversService.Value)
                    : new TestSmtpMessageSender(this.settings.Value, this.rewriteReceiversService.Value);
        }

        /// <summary>
        /// Переопределять только если на проде ASPNETCORE_ENVIRONMENT не Production и нужен workaround.
        /// И в целом тут лучше повнимательнее быть - чтобы пользователей письмами с тестовых стендов не заспамить
        /// </summary>
        protected virtual bool IsProduction()
            => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;

        public void Send(NotificationEventDTO message, TransactionMessageMode sendMessageMode = TransactionMessageMode.Auto)
        {
            using (var client = this.GetSmtpClient())
            {
                try
                {
                    this.sender.Value.Send(client, message);

                    new SentMessageBLL(this.context).Save(message.ToSentMessage());
                }
                catch (Exception e)
                {
                    Log.Error(e, "Failed to send notification to smtp server");

                    throw e;
                }
            }
        }

        // todo: перевести создание SmtpClient в DI 
        protected virtual SmtpClient GetSmtpClient()
        {
            if (!this.settings.Value.SmtpEnabled && string.IsNullOrWhiteSpace(this.settings.Value.OutputFolder))
            {
                throw new ConfigurationErrorsException("Please enable smtp or specify local output folder for sent notifications");
            }

            return this.settings.Value.SmtpEnabled
                ? new SmtpClient(this.settings.Value.Server, this.settings.Value.Port) { UseDefaultCredentials = true }
                : new SmtpClient
                {
                    UseDefaultCredentials = true,
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = this.settings.Value.OutputFolder
                };
        }
    }
}
