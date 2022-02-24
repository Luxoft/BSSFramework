using System.Collections.Generic;

using Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Notification;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Templates
{
    internal abstract class MessageTemplateFactoryBase
    {
        internal abstract IEnumerable<MessageTemplateNotification> Create<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> versions,
            Subscription subscription,
            RecipientsBag recipientsBag,
            IEnumerable<Attachment> attachments)
            where TModelObjectType : class
            where TSourceDomainObjectType: class;

        protected MessageTemplateNotification CreateTemplate<TSourceDomainObjectType, TModelObjectType>(
            DomainObjectVersions<TModelObjectType> versions,
            Subscription subscription,
            IEnumerable<string> to,
            IEnumerable<string> cc,
            IEnumerable<string> replyTo,
            IEnumerable<Attachment> attachments)
            where TModelObjectType : class
        {
            var template = new MessageTemplateNotification(
                subscription.MessageTemplate.Code,
                versions,
                versions.DomainObjectType,
                to,
                cc,
                replyTo,
                attachments,
                subscription,
                subscription.AllowEmptyListOfRecipients,
                typeof(TSourceDomainObjectType));

            template.RazorMessageTemplateType = subscription.RazorMessageTemplateType;

            return template;
        }
    }
}
