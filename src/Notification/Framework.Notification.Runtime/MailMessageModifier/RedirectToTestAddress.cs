using System.Net.Mail;

using Anch.Core;

using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.MailMessageModifier;

public class RedirectToTestAddress(IOptions<SmtpSettings> settings) : IMailMessageModifier
{
    /// <summary>
    /// Test-environment safety net: when <c>TestEmails</c> are configured, ALL real recipients (To/CC/Bcc/ReplyTo) are removed
    /// and the message goes only to the test addresses. The original recipients are prepended to the body so testers can see them;
    /// the body is assumed to be HTML (<c>&lt;br&gt;</c>) and Bcc is intentionally not shown there.
    /// Modifiers that run after this one see only the test addresses.
    /// </summary>
    public void Modify(MailMessage message)
    {
        if (settings.Value.TestEmails.Any())
        {
            this.AddRecipientsToBody(message);

            message.To.Clear();
            message.CC.Clear();
            message.Bcc.Clear();
            message.ReplyToList.Clear();

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
