using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Framework.Subscriptions;

public static class InlineAttachmentHelper
{
    public static void InlineAttachment(MailMessage message, Attachment attachment) =>
        message.Body = ReplaceSrcByName(message.Body, attachment.Name!, attachment.ContentId);

    private static string ReplaceSrcByName(string body, string name, string contentId)
    {
        var pattern = $"src\\s*=\\s*\"{name}\"";

        return Regex.Replace(body, pattern, $"src=\"cid:{contentId}\"", RegexOptions.IgnoreCase);
    }
}
