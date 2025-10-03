using System.Collections.ObjectModel;
using System.Net.Mail;

using CommonFramework;

using Framework.Core;

namespace Framework.Notification.New;

public class Message
{
    public Message(string sender, IEnumerable<string> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender.ToMailAddress(), receivers.ToReceiverAddresses(), subject, body, isBodyHtml, attachments)
    {

    }

    public Message(string sender, IEnumerable<ReceiverAddress> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender.ToMailAddress(), receivers, subject, body, isBodyHtml, attachments)
    {

    }

    public Message(MailAddress sender, IEnumerable<string> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
            : this(sender, receivers.ToReceiverAddresses(), subject, body, isBodyHtml, attachments)
    {

    }

    public Message(MailAddress sender, IEnumerable<ReceiverAddress> receivers, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments)
    {
        if (sender == null) throw new ArgumentNullException(nameof(sender));
        if (subject == null) throw new ArgumentNullException(nameof(subject));
        if (body == null) throw new ArgumentNullException(nameof(body));
        if (receivers == null) throw new ArgumentNullException(nameof(receivers));
        if (attachments == null) throw new ArgumentNullException(nameof(attachments));

        this.Sender = sender;
        this.Receivers = receivers.ToReadOnlyCollection();

        if (!this.Receivers.Any())
        {
            throw new ArgumentException("Collection 'Receivers' is empty", nameof(receivers));
        }

        this.Subject = this.CutSubjectOnRight(this.ReplaceUnsupportedCharactersForSubject(subject));
        this.Body = body;
        this.IsBodyHtml = isBodyHtml;
        this.Attachments = attachments.ToReadOnlyCollection();
    }


    public MailAddress Sender { get; private set; }

    public ReadOnlyCollection<ReceiverAddress> Receivers { get; private set; }

    public string Subject { get; private set; }

    public string Body { get; private set; }

    public bool IsBodyHtml { get; private set; }

    public ReadOnlyCollection<Attachment> Attachments { get; private set; }

    public override string ToString()
    {
        return $"Sender: {this.Sender} | Receivers: {this.Receivers.Join(", ")} | Subject: {this.Subject} | Body: {this.Body} | Attachments: {this.Attachments.Join(", ")}";
    }

    private string CutSubjectOnRight(string subject)
    {
        var recommendationLimitCharactersInSubject = 78;
        return subject.SubStringUnsafe(recommendationLimitCharactersInSubject);
    }

    private string ReplaceUnsupportedCharactersForSubject(string subject)
    {
        return subject.Replace(Environment.NewLine, " ").Replace('\r', ' ').Replace('\n', ' ');
    }
}
