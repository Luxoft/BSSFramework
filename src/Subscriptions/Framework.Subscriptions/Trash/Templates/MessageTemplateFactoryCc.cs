using Framework.Subscriptions.Domain;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Subscriptions.Templates;

internal sealed class MessageTemplateFactoryCc : MessageTemplateFactoryBase
{
    internal override IEnumerable<MessageTemplateNotification> Create<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> versions,
            Subscription subscription,
            RecipientsBag recipientsBag,
            IEnumerable<Attachment> attachments)
    {
        var template = this.CreateTemplate<TSourceDomainObjectType, TModelObjectType>(
                                                                                      versions,
                                                                                      subscription,
                                                                                      recipientsBag.To.Select(r => r.Email),
                                                                                      recipientsBag.Cc.Select(r => r.Email),
                                                                                      recipientsBag.ReplyTo.Select(r => r.Email),
                                                                                      attachments);

        yield return template;
    }
}
