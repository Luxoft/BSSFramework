using System.Net.Mail;

namespace Framework.Notification.Domain;

public record NotificationTarget(MailAddress MailAddress, ReceiverRole Role)
{
    public NotificationTarget(string address, ReceiverRole role)
        : this(new MailAddress(address), role)
    {
    }

    public override string ToString() => $"{this.MailAddress} ({nameof(this.Role)} = {this.Role})";
}
