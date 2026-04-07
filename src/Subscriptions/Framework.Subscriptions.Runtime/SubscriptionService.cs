using System.Collections.Immutable;
using System.Net.Mail;

using CommonFramework;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class SubscriptionService(IEnumerable<ISubscription> subscriptionMetadataList) : ISubscriptionService
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions versions)
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }

    private IEnumerable<ITryResult<SubscriptionHeader>> Process<TDomainObject>(DomainObjectVersions<TDomainObject> versions)
        where TDomainObject : class
    {
        var x1 = subscriptionMetadataList.Where(sm => sm.DomainObjectChangeType.HasFlag(versions.ChangeType) && sm.DomainObjectType == versions.DomainObjectType).ToList();



        return [];
    }
}

public class SubscriptionService<TDomainObject, TRenderingObject>(
    IServiceProvider serviceProvider,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    IEmployeeEmailExtractor employeeEmailExtractor,
    IMessageSender<MailMessage> mailMessageSender,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null)
    where TDomainObject : class
    where TRenderingObject : class
{
    public IEnumerable<ITryResult<SubscriptionHeader>> Process(DomainObjectVersions<TDomainObject> versions)
    {
        var mailMessages = this.GetMailMessages(versions);

        foreach (var mailMessage in mailMessages)
        {
            defaultCancellationTokenSource.RunSync(async ct => await mailMessageSender.SendAsync(mailMessage, ct));
        }

        yield return TryResult.Return(subscription.Header);
    }

    private IEnumerable<Notification.Domain.Notification> GetMailMessages(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.IsProcessed(serviceProvider, versions))
        {
            var preTo = subscription.GetTo(serviceProvider, versions);
            var authTo = this.GetAuthTo(versions);

            var resultTo = this.GetMergeResult(preTo, authTo);
            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);

            var regrouped = ReGroup(resultTo, copyTo, replyTo);

            return regrouped.Select(this.ToNotification);
        }
        else
        {
            return [];
        }
    }

    private Notification.Domain.Notification ToNotification(NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient> notificationMessageGenerationInfo)
    {
        var (subject, body) = subscription.GetMessage(serviceProvider, notificationMessageGenerationInfo.Versions);

        var attachments = subscription.GetAttachments(serviceProvider, notificationMessageGenerationInfo.Versions);

        var mailMessage = new MailMessage
                          {
                              Sender = subscription.Sender,
                              Subject = subject,
                              Body = body,
                              Recipients = [.. notificationMessageGenerationInfo.Recipients],
                              AttachmentList = [.. attachments]
                          };

        var notificationTechnicalInformation = new NotificationTechnicalInformation(
            subscription.MessageTemplateCode,
            typeof(TDomainObject).FullName!,

            );

        return new Notification.Domain.Notification()
    }

    private static IEnumerable<NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>> ReGroup(
        IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> to,
        IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> copyTo,
        IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> replyTo) =>

        from g in new[] { to.GroupRecipients(RecipientRole.To), copyTo.GroupRecipients(RecipientRole.Copy), replyTo.GroupRecipients(RecipientRole.ReplyTo) }.RegroupRecipients()

        let recipients = g.Select(pair => new NotificationRecipient(pair.Recipient, pair.Tag))

        select new NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>([.. recipients], g.Key);

    private IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetAuthTo(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.SecurityRoles.Length > 0)
        {
            var notificationFilterGroups = subscription.GetNotificationFilterGroups(serviceProvider, versions).ToImmutableArray();

            if (notificationFilterGroups.Length > 0)
            {
                var emails = employeeEmailExtractor.GetEmails(subscription.SecurityRoles, notificationFilterGroups);

                if (emails.Count > 0)
                {
                    var renderingVersions = versions.ChangeDomainObject(domainObject => subscription.ConvertToRenderingObject(serviceProvider, domainObject));

                    yield return new NotificationMessageGenerationInfo<TRenderingObject>(emails, renderingVersions);
                }
            }
        }
    }

    private IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetMergeResult(
        IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> preTo,
        IEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> authTo) =>

        from g in new[] { preTo.GroupRecipients(false), authTo.GroupRecipients(true) }.RegroupRecipients()

        let resultRecipients = g.Partial(
            pair => pair.Tag,
            (r1, r2) =>
                r1.Select(pair => pair.Recipient)
                  .GetEmailMergeResult(r2.Select(pair => pair.Recipient), subscription.RecipientMergeType))

        select new NotificationMessageGenerationInfo<TRenderingObject>([.. resultRecipients], g.Key);
}
