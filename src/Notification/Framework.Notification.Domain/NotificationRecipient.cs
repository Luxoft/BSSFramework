using System.Net.Mail;

namespace Framework.Notification.Domain;

public record NotificationRecipient(MailAddress Address, RecipientRole Role)
{
    public NotificationRecipient(string address, RecipientRole role)
        : this(new MailAddress(address), role)
    {
    }

    public override string ToString() => $"{this.Address} ({nameof(this.Role)} = {this.Role})";
}
