using System.Net.Mail;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public static class MailMessageExtensions
{
    public static SentMessage ToSentMessage(this MailMessage mailMessage) =>

        new(
            mailMessage.From!.Address,
            string.Join(",", mailMessage.To.Select(z => z.Address)),
            mailMessage.Subject,
            mailMessage.Body,
            "",
            string.Empty,
            string.Join(",", mailMessage.CC.Select(z => z.Address)),
            "",
            null,
            string.Join(",", mailMessage.ReplyToList.Select(z => z.Address)));
}
