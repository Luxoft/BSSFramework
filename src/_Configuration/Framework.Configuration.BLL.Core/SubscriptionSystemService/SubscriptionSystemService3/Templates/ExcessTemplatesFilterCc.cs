using System.Collections.Generic;
using System.Linq;
using Framework.Notification;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates
{
    internal sealed class ExcessTemplatesFilterCc : ExcessTemplatesFilterBase
    {
        internal IEnumerable<MessageTemplateNotification> ProcessTemplates(
            IEnumerable<MessageTemplateNotification> templates)
        {
            var groups = CollapseTemplates(GetTemplatesCc(templates));
            var result = groups.Select(@group => CreateCommonTemplate(@group.ToList()));

            return result;
        }

        private static MessageTemplateNotification CreateCommonTemplate(
            IReadOnlyCollection<MessageTemplateNotification> templates)
        {
            var commonTemplate = FindCommonTemplate(templates);
            var toRecipients = templates.SelectMany(t => t.Receivers).Distinct();
            var ccRecipients = templates.SelectMany(t => t.CopyReceivers).Distinct();
            var replyTo = templates.SelectMany(t => t.ReplyTo).Distinct();
            var result = CopyTemplate(commonTemplate, toRecipients, ccRecipients, replyTo);

            return result;
        }

        private static MessageTemplateNotification FindCommonTemplate(
            IEnumerable<MessageTemplateNotification> templates)
        {
            return templates.OrderByDescending(t => t.Subscription.IncludeAttachments).First();
        }
    }
}
