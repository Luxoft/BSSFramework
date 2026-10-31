using System.Net.Mail;

namespace Framework.Notification.MailMessageModifier;

public class GdprMailMessageModifier : IMailMessageModifier
{
    private const string StartToken = "<!--GDPR-->";

    private const string EndToken = "<!--/GDPR-->";

    /// <summary>
    /// Cuts out every <c>&lt;!--GDPR--&gt;...&lt;!--/GDPR--&gt;</c> section from the body (tokens are case-insensitive).
    /// Sections are removed starting from the LAST opening token, so nested sections collapse correctly from the inside out.
    /// An opening token without a closing one stops processing and the rest of the text stays as is - the unclosed part is NOT removed.
    /// </summary>
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
