using System.Net.Mail;

namespace Framework.Notification.Domain;

public record MailAddressRecipient(MailAddress Address, RecipientRole Role)
{
    public MailAddressRecipient(string address, RecipientRole role)
        : this(new MailAddress(address), role)
    {
    }

    public override string ToString() => $"{this.Address} ({nameof(this.Role)} = {this.Role})";
}
