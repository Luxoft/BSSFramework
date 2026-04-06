using System.Collections.Immutable;
using System.Net.Mail;

using CommonFramework;

namespace Framework.Notification.Domain;

public class Message
{
    public Message(string sender, IEnumerable<MailAddressRecipient> recipients, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(new MailAddress(sender), recipients, subject, body, isBodyHtml, attachments)
    {
    }

    public Message(MailAddress sender, IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender, recipients.Select(receiver => new MailAddressRecipient(receiver, RecipientRole.To)), subject, body, isBodyHtml, attachments)
    {
    }

    public Message(MailAddress sender, IEnumerable<MailAddressRecipient> recipients, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        if (subject == null) throw new ArgumentNullException(nameof(subject));
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (recipients == null) throw new ArgumentNullException(nameof(recipients));
        if (attachments == null) throw new ArgumentNullException(nameof(attachments));

        this.Sender = sender;
        this.Recipients = [..recipients];

        if (!this.Recipients.Any())
        {
            throw new ArgumentException("Collection 'Receivers' is empty", nameof(recipients));
        }

        this.Subject = subject;
        this.Body = body;
        this.IsBodyHtml = isBodyHtml;
        this.Attachments = [..attachments];
    }


    public MailAddress Sender { get; }

    public ImmutableArray<MailAddressRecipient> Recipients { get; }

    public string Subject { get; }

    public string Body { get; }

    public bool IsBodyHtml { get; }

    public ImmutableArray<Attachment> Attachments { get; }

    public override string ToString() => $"Sender: {this.Sender} | Receivers: {this.Recipients.Join(", ")} | Subject: {this.Subject} | Body: {this.Body} | Attachments: {this.Attachments.Join(", ")}";



    public MailMessage ToMailMessage()
    {
        var mailMessage = new MailMessage { Subject = this.Subject, Body = this.Body, From = this.Sender, IsBodyHtml = this.IsBodyHtml };

        mailMessage.To.AddRange(this.Recipients.Where(r => r.Role == RecipientRole.To).Select(info => info.Address));
        mailMessage.CC.AddRange(this.Recipients.Where(r => r.Role == RecipientRole.Copy).Select(info => info.Address));
        mailMessage.ReplyToList.AddRange(this.Recipients.Where(r => r.Role == RecipientRole.ReplyTo).Select(info => info.Address));

        mailMessage.Attachments.AddRange(this.Attachments.Select(a => a.ToMailAttachment()));

        return mailMessage;
    }
}
