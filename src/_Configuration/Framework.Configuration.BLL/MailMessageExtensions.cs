using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public static class MailMessageExtensions
{
    public static SentMessage ToSentMessage(this Framework.Notification.Domain.Notification notification) =>

        new(
            notification.Message.From!.Address,
            string.Join(",", notification.Message.To.Select(z => z.Address)),
            notification.Message.Subject,
            notification.Message.Body,
            notification.TechnicalInformation.MessageTemplateCode,
            "",
            string.Join(",", notification.Message.CC.Select(z => z.Address)),
            notification.TechnicalInformation.ContextObjectType,
            notification.TechnicalInformation.ContextObjectId,
            string.Join(",", notification.Message.ReplyToList.Select(z => z.Address)));
}
