using System.Net.Mail;

namespace Framework.Notification.MailMessageModifier;

public class GdprCleanerModifier : IMailMessageModifier
{
    private const string StartToken = "<!--GDPR-->";

    private const string EndToken = "<!--/GDPR-->";

    public void Modify(MailMessage message)
    {
        var messageText = message.Body;
        if (string.IsNullOrEmpty(messageText))
        {
            return;
        }

        var searchFrom = messageText.Length;

        while (true)
        {
            var startIndex = messageText.LastIndexOf(StartToken, Math.Max(searchFrom - 1, 0), StringComparison.OrdinalIgnoreCase);
            if (startIndex < 0)
            {
                break;
            }

            var endIndex = messageText.IndexOf(EndToken, startIndex, StringComparison.OrdinalIgnoreCase);
            if (endIndex < 0)
            {
                searchFrom = startIndex;
                continue;
            }

            messageText = messageText.Remove(startIndex, endIndex + EndToken.Length - startIndex);
            searchFrom = messageText.Length;
        }

        message.Body = messageText;
    }
}
