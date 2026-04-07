using System.Net.Mail;

using CommonFramework;

using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RedirectToTestAddress(IOptions<SmtpSettings> settings) : IMailMessageModifier
{
    public void Modify(MailMessage message)
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
