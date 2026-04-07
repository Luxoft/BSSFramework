using System.Net.Mail;

using CommonFramework;

using Framework.Notification.Extensions;
using Framework.Notification.Settings;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Notification;

/// <summary>
/// логика для тестовых стендов - письма перенаправляются на тестовый почтовый ящик, а исходные адресаты записываются в тело письма
/// </summary>
public class TestSmtpMessageSender(
    ISmtpClientFactory smtpClientFactory,
    IOptions<SmtpSettings> settings,
    ILogger<ProdSmtpMessageSender> logger,
    IRewriteReceiversService rewriteReceiversService,
    ISentMessageLogger? sentMessageLogger = null)
    : ProdSmtpMessageSender(smtpClientFactory, settings, logger, sentMessageLogger)
{
    protected override MailMessage GetActualMailMessage(MailMessage baseMessage)
    {
        var newMessage = base.GetActualMailMessage(baseMessage);

        rewriteReceiversService.RewriteRecipients(newMessage);

        this.AddRedirectToTestAddress(newMessage);

        return newMessage;
    }

    private void AddRedirectToTestAddress(MailMessage message)
    {
        this.AddRecipientsToBody(message);

        message.To.Clear();
        message.CC.Clear();
        message.Bcc.Clear();
        message.ReplyToList.Clear();

        if (settings.Value.TestEmails.Any())
        {
            message.To.AddRange(RecipientsHelper.ToRecipients(settings.Value.TestEmails));
        }
    }

    private void AddRecipientsToBody(MailMessage message)
    {
        var originalReceivers =
                $"From: {message.From!.Address}<br>" +
                $"To: {message.To.Select(x => x.Address).Join("; ")}<br>" +
                $"CC: {message.CC.Select(x => x.Address).Join("; ")}<br>" +
                $"Reply To: {message.ReplyToList.Select(x => x.Address).Join("; ")}<br><br>";

        message.Body = $"{originalReceivers}{message.Body}";
    }
}
