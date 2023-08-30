using System.Configuration;
using System.Net.Mail;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Notification.DTO;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

using Serilog;

namespace Framework.NotificationCore.Senders;

public class SmtpNotificationMessageSender : IMessageSender<NotificationEventDTO>
{
    private readonly IConfigurationBLLContext context;

    private readonly IOptions<SmtpSettings> settings;

    private readonly IRewriteReceiversService rewriteReceiversService;

    private readonly ISmtpMessageSender sender;

    public SmtpNotificationMessageSender(
            IOptions<SmtpSettings> settings,
            IRewriteReceiversService rewriteReceiversService,
            IConfigurationBLLContext context)
    {
        this.context = context;

        this.settings = settings;

        this.rewriteReceiversService = rewriteReceiversService;

        this.sender = this.GetSender();
    }

    private ISmtpMessageSender GetSender() =>
            this.IsProduction()
                    ? new ProdSmtpMessageSender(this.settings.Value, this.rewriteReceiversService)
                    : new TestSmtpMessageSender(this.settings.Value, this.rewriteReceiversService);

    /// <summary>
    /// Переопределять только если на проде ASPNETCORE_ENVIRONMENT не Production и нужен workaround.
    /// И в целом тут лучше повнимательнее быть - чтобы пользователей письмами с тестовых стендов не заспамить
    /// </summary>
    protected virtual bool IsProduction()
        => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Production;

    public void Send(NotificationEventDTO message)
    {
        using var client = this.GetSmtpClient();

        try
        {
            this.sender.Send(client, message);

            this.SaveSentMessage(message.ToSentMessage());
        }
        catch (Exception e)
        {
            Log.Error(e, "Failed to send notification to smtp server");

            throw;
        }
    }

    protected virtual void SaveSentMessage(SentMessage message) =>
        this.context.Logics.SentMessage.Save(message);

    // TODO: Move creation of SmtpClient to DI
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
