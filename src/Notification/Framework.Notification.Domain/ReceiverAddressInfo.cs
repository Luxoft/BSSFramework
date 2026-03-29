using System.Net.Mail;

namespace Framework.Notification.Domain;

public record ReceiverAddressInfo(MailAddress MailAddress, ReceiverRole Role)
{
    public override string ToString() => $"{this.MailAddress} ({nameof(this.Role)} = {this.Role})";
}
