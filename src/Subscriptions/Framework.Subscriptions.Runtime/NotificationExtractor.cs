using System.Collections.Immutable;
using System.Net.Mail;

using CommonFramework;
using CommonFramework.IdentitySource;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class NotificationExtractor<TDomainObject, TRenderingObject>(
    IServiceProvider serviceProvider,
    IIdentityInfoSource identityInfoSource,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    IEmployeeEmailExtractor employeeEmailExtractor) : INotificationExtractor
    where TDomainObject : class
    where TRenderingObject : class
{
    public IEnumerable<Notification.Domain.Notification> GetNotifications(DomainObjectVersions versions)
    {
        var typedVersions = (DomainObjectVersions<TDomainObject>)versions;

        var technicalInformation = new NotificationTechnicalInformation(
            subscription.MessageTemplateCode,
            typeof(TDomainObject).FullName!,
            identityInfoSource.TryGetIdentityInfo<TDomainObject>()
                              .Maybe(identityInfo => identityInfo.GetId(typedVersions.Previous ?? typedVersions.Current!) as Guid?));

        return from mailMessage in this.GetMailMessages(typedVersions)

               select new Notification.Domain.Notification(technicalInformation, mailMessage);
    }

    private IEnumerable<MailMessage> GetMailMessages(DomainObjectVersions<TDomainObject> versions)
    {
        if (subscription.IsProcessed(serviceProvider, versions))
        {
            var preTo = subscription.GetTo(serviceProvider, versions);
            var authTo = this.GetAuthTo(versions);

            var resultTo = this.GetMergeResult(preTo, authTo);
            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);

            return ReGroup(resultTo, copyTo, replyTo).Select(this.ToMailMessage);
        }
        else
        {
            return [];
        }
    }

    private MailMessage ToMailMessage(NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient> notificationMessageGenerationInfo)
    {
        var (subject, body) = subscription.GetMessage(serviceProvider, notificationMessageGenerationInfo.Versions);

        var attachments = subscription.GetAttachments(serviceProvider, notificationMessageGenerationInfo.Versions);

        return new MailMessage
               {
                   From = subscription.Sender,
                   Subject = subject,
                   Body = body,
                   Recipients = [.. notificationMessageGenerationInfo.Recipients],
                   AttachmentList = [.. attachments]
               };
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
                Enumerable.Select<(string Recipient, bool Tag), string>(r1, pair => pair.Recipient)
                          .GetEmailMergeResult(Enumerable.Select<(string Recipient, bool Tag), string>(r2, pair => pair.Recipient), subscription.RecipientMergeType))

        select new NotificationMessageGenerationInfo<TRenderingObject>([.. resultRecipients], g.Key);
}
