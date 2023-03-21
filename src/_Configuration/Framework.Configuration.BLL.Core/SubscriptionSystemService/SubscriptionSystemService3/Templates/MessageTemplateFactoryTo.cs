using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates;

internal sealed class MessageTemplateFactoryTo : MessageTemplateFactoryBase
{
    internal override IEnumerable<MessageTemplateNotification> Create<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> versions,
            Subscription subscription,
            RecipientsBag recipientsBag,
            IEnumerable<Attachment> attachments)
    {
        return subscription.SendIndividualLetters
                       ? this.CreateTemplatesForEach<TSourceDomainObjectType, TModelObjectType>(versions, subscription, recipientsBag.To, recipientsBag.ReplyTo, attachments)
                       : this.CreateOneTemplateForAll<TSourceDomainObjectType, TModelObjectType>(versions, subscription, recipientsBag.To, recipientsBag.ReplyTo, attachments);
    }

    private IEnumerable<MessageTemplateNotification> CreateTemplatesForEach<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> domainObjectVersions,
            Subscription subscription,
            RecipientCollection recipients,
            RecipientCollection replyTo,
            IEnumerable<Attachment> attachments)
            where TModelObjectType : class
    {
        var result = recipients.Select(
                                       recipient => this.CreateTemplate<TSourceDomainObjectType, TModelObjectType>(
                                        domainObjectVersions,
                                        subscription,
                                        new[] { recipient.Email },
                                        Enumerable.Empty<string>(),
                                        replyTo.Select(z=>z.Email),
                                        attachments));

        return result;
    }

    private IEnumerable<MessageTemplateNotification> CreateOneTemplateForAll<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> domainObjectVersions,
            Subscription subscription,
            RecipientCollection recipients,
            RecipientCollection replyTo,
            IEnumerable<Attachment> attachments)
            where TModelObjectType : class
    {
        var result = this.CreateTemplate<TSourceDomainObjectType, TModelObjectType>(
                                                                                    domainObjectVersions,
                                                                                    subscription,
                                                                                    recipients.Select(r => r.Email),
                                                                                    Enumerable.Empty<string>(),
                                                                                    replyTo.Select(z => z.Email),
                                                                                    attachments);

        yield return result;
    }
}
