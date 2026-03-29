using Framework.Notification.Domain;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates;

internal sealed class ExcessTemplatesFilterTo : ExcessTemplatesFilterBase
{
    internal IEnumerable<MessageTemplateNotification> ProcessTemplates(
            IEnumerable<MessageTemplateNotification> templates)
    {
        var groups = CollapseTemplates(GetTemplatesTo(templates));
        var result = groups.SelectMany(ProcessTemplatesGroup);

        return result;
    }

    private static IEnumerable<MessageTemplateNotification> ProcessTemplatesGroup(
            IGrouping<MessageTemplateNotification, MessageTemplateNotification> group)
    {
        return CreateTemplates(group.ToList());
    }

    private static IEnumerable<MessageTemplateNotification> CreateTemplates(
            IReadOnlyCollection<MessageTemplateNotification> templates)
    {
        var commonTemplate = FindCommonTemplate(templates);
        var recipients = templates.SelectMany(t => t.Receivers).Distinct();

        var result = commonTemplate.Subscription.SendIndividualLetters
                             ? CreateTemplatesForEach(commonTemplate, recipients)
                             : CreateTemplatesForAll(commonTemplate, recipients);

        return result;
    }

    private static IEnumerable<MessageTemplateNotification> CreateTemplatesForEach(
            MessageTemplateNotification commonTemplate, IEnumerable<string> recipients)
    {

        var result = recipients.Select(r => CopyTemplate(
                                                         commonTemplate,
                                                         [r],
                                                         [],
                                                         commonTemplate.ReplyTo));

        return result;
    }

    private static IEnumerable<MessageTemplateNotification> CreateTemplatesForAll(
            MessageTemplateNotification commonTemplate, IEnumerable<string> recipients)
    {
        var result = CopyTemplate(commonTemplate, recipients, [], commonTemplate.ReplyTo);
        yield return result;
    }

    private static MessageTemplateNotification FindCommonTemplate(
            IEnumerable<MessageTemplateNotification> templates)
    {
        return templates
               .OrderByDescending(t => t.Subscription.SendIndividualLetters)
               .ThenByDescending(t => t.Subscription.IncludeAttachments)
               .First();
    }
}
