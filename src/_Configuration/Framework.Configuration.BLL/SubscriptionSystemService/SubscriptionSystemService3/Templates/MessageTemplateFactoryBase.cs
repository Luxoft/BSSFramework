using Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Recipients;
using Framework.Configuration.Domain;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;

using Attachment = System.Net.Mail.Attachment;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Templates;

internal abstract class MessageTemplateFactoryBase
{
    internal abstract IEnumerable<MessageTemplateNotification> Create<TSourceDomainObjectType, TModelObjectType>(
        DomainObjectVersions<TModelObjectType> versions,
        Subscription subscription,
        RecipientsBag recipientsBag,
        IEnumerable<Attachment> attachments)
        where TModelObjectType : class
        where TSourceDomainObjectType : class;

    protected MessageTemplateNotification CreateTemplate<TSourceDomainObjectType, TModelObjectType>(
        DomainObjectVersions<TModelObjectType> versions,
        Subscription subscription,
        IEnumerable<string> to,
        IEnumerable<string> cc,
        IEnumerable<string> replyTo,
        IEnumerable<Attachment> attachments)
        where TModelObjectType : class =>
        new()
        {
            MessageTemplateCode = subscription.RazorMessageTemplateType.FullName!,
            ContextObject = versions,
            ContextObjectType = versions.DomainObjectType,
            Receivers = [.. to],
            CopyReceivers = [.. cc],
            ReplyTo = [.. replyTo],
            Attachments = [..attachments],
            Subscription = subscription,
            SendWithEmptyListOfRecipients = subscription.AllowEmptyListOfRecipients,
            SourceContextObjectType = typeof(TSourceDomainObjectType),
            RazorMessageTemplateType = subscription.RazorMessageTemplateType
        };
}
