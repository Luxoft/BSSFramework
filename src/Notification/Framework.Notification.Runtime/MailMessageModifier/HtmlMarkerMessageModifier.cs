using System.Net.Mail;

namespace Framework.Notification.MailMessageModifier;

public class HtmlMarkerMessageModifier : IMailMessageModifier
{
    public void Modify(MailMessage message) => message.IsBodyHtml = message.Body.StartsWith("<html");
}
