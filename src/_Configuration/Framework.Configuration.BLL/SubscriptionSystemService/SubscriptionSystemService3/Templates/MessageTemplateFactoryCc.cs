using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates;

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
