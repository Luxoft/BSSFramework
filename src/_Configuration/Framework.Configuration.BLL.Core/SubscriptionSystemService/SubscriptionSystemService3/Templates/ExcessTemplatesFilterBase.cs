using Framework.Notification;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates;

internal class ExcessTemplatesFilterBase
{
    private static readonly MessageTemplateNotificationComparer TemplateComparer =
            new MessageTemplateNotificationComparer();

    protected ExcessTemplatesFilterBase()
    {
    }

    public static MessageTemplateNotification CopyTemplate(
            MessageTemplateNotification template,
            IEnumerable<string> toRecipients,
            IEnumerable<string> ccRecipients,
            IEnumerable<string> replyTo)
    {
        var result = new MessageTemplateNotification(
                                                     template.MessageTemplateCode,
                                                     template.ContextObject,
                                                     template.ContextObjectType,
                                                     toRecipients,
                                                     ccRecipients,
                                                     replyTo,
                                                     template.Attachments,
                                                     template.Subscription,
                                                     template.SendWithEmptyListOfRecipients,
                                                     template.SourceContextObjectType);

        result.RazorMessageTemplateType = template.RazorMessageTemplateType;

        return result;
    }

    public static IEnumerable<IGrouping<MessageTemplateNotification, MessageTemplateNotification>> CollapseTemplates(
            IEnumerable<MessageTemplateNotification> templates)
    {
        return templates.GroupBy(t => t, TemplateComparer);
    }

    public static IEnumerable<MessageTemplateNotification> GetTemplatesTo(
            IEnumerable<MessageTemplateNotification> templates)
    {
        return templates.Where(t => !t.CopyReceivers.Any());
    }

    public static IEnumerable<MessageTemplateNotification> GetTemplatesCc(
            IEnumerable<MessageTemplateNotification> templates)
    {
        return templates.Where(t => t.CopyReceivers.Any());
    }

    private class MessageTemplateNotificationComparer : IEqualityComparer<MessageTemplateNotification>
    {
        public bool Equals(MessageTemplateNotification x, MessageTemplateNotification y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (x.GetType() != y.GetType())
            {
                return false;
            }

            return string.Equals(x.MessageTemplateCode, y.MessageTemplateCode) &&
                   Equals(x.ContextObject, y.ContextObject);
        }

        public int GetHashCode(MessageTemplateNotification obj)
        {
            unchecked
            {
                var result = ((obj.MessageTemplateCode?.GetHashCode() ?? 0) * 397) ^
                             (obj.ContextObject?.GetHashCode() ?? 0);

                return result;
            }
        }
    }
}
