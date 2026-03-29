using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Net.Mail;

using CommonFramework;

using Framework.Core;

namespace Framework.Notification.Domain;

public class Message
{
    public Message(string sender, IEnumerable<string> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender.ToMailAddress(), receivers.ToReceiverAddresses(), subject, body, isBodyHtml, attachments)
    {
    }

    public Message(string sender, IEnumerable<ReceiverAddressInfo> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender.ToMailAddress(), receivers, subject, body, isBodyHtml, attachments)
    {
    }

    public Message(MailAddress sender, IEnumerable<string> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender, receivers.ToReceiverAddresses(), subject, body, isBodyHtml, attachments)
    {
    }

    public Message(MailAddress sender, IEnumerable<ReceiverAddressInfo> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        if (subject == null) throw new ArgumentNullException(nameof(subject));
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (receivers == null) throw new ArgumentNullException(nameof(receivers));
        if (attachments == null) throw new ArgumentNullException(nameof(attachments));

        this.Sender = sender;
        this.Receivers = [..receivers];

        if (!this.Receivers.Any())
        {
            throw new ArgumentException("Collection 'Receivers' is empty", nameof(receivers));
        }

        this.Subject = this.CutSubjectOnRight(this.ReplaceUnsupportedCharactersForSubject(subject));
        this.Body = body;
        this.IsBodyHtml = isBodyHtml;
        this.Attachments = [..attachments];
    }


    public MailAddress Sender { get; }

    public ImmutableArray<ReceiverAddressInfo> Receivers { get; }

    public string Subject { get; }

    public string Body { get; }

    public bool IsBodyHtml { get; private set; }

    public ImmutableArray<Attachment> Attachments { get; }

    public override string ToString() => $"Sender: {this.Sender} | Receivers: {this.Receivers.Join(", ")} | Subject: {this.Subject} | Body: {this.Body} | Attachments: {this.Attachments.Join(", ")}";

    private string CutSubjectOnRight(string subject)
    {
        var recommendationLimitCharactersInSubject = 78;
        return subject.SubStringUnsafe(recommendationLimitCharactersInSubject);
    }

    private string ReplaceUnsupportedCharactersForSubject(string subject) => subject.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
}
