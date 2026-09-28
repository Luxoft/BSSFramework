using System.Net.Mail;

using Framework.Notification.MailMessageModifier;

namespace Framework.Notification;

public class GdprMailMessageModifier : IMailMessageModifier
{
    private const string StartToken = "<!--GDPR-->";

    private const string EndToken = "<!--/GDPR-->";

    public void Modify(MailMessage baseMessage)
    {
        var messageText = baseMessage.Body;

        do
        {
            var startIndex = messageText.LastIndexOf(StartToken, StringComparison.OrdinalIgnoreCase);
            if (startIndex < 0)
            {
                break;
            }

            var endIndex = messageText.IndexOf(EndToken, startIndex, StringComparison.OrdinalIgnoreCase);
            if (endIndex < 0)
            {
                break;
            }

            messageText = messageText.Remove(startIndex, endIndex + EndToken.Length - startIndex);
        } while (true);

        baseMessage.Body = messageText;
    }
}
