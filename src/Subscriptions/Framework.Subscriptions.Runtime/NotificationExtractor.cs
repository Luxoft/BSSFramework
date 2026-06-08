using System.Net.Mail;
using System.Runtime.CompilerServices;

using Anch.Core;
using Anch.IdentitySource;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public class NotificationExtractor<TDomainObject, TRenderingObject>(
    IServiceProvider serviceProvider,
    IIdentityInfoSource identityInfoSource,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    IEmployeeEmailExtractor employeeEmailExtractor) : INotificationExtractor<TDomainObject>
    where TDomainObject : class
    where TRenderingObject : class
{
    public IAsyncEnumerable<Notification.Domain.Notification> GetNotifications(DomainObjectVersions<TDomainObject> versions)
    {
        var technicalInformation = new NotificationTechnicalInformation(
            subscription.MessageTemplateCode,
            typeof(TDomainObject).FullName!,
            identityInfoSource.TryGetIdentityInfo<TDomainObject>()
                              .Maybe(identityInfo => identityInfo.GetId(versions.Previous ?? versions.Current!) as Guid?));

        return from mailMessage in this.GetMailMessages(versions)

               select new Notification.Domain.Notification(technicalInformation, mailMessage);
    }

    private async IAsyncEnumerable<MailMessage> GetMailMessages(DomainObjectVersions<TDomainObject> versions, [EnumeratorCancellation] CancellationToken ct = default)
    {
        if (await subscription.IsProcessed(serviceProvider, versions, ct))
        {
            var preTo = subscription.GetTo(serviceProvider, versions);
            var authTo = this.GetAuthTo(versions, ct);

            var resultTo = this.GetMergeResult(preTo, authTo);
            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);

            await foreach (var mailMessage in ReGroup(resultTo, copyTo, replyTo).Select(this.ToMailMessage).WithCancellation(ct))
            {
                yield return mailMessage;
            }
        }
    }

    private async ValueTask<MailMessage> ToMailMessage(
        NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient> notificationMessageGenerationInfo,
        CancellationToken ct)
    {
        var (subject, body) = await subscription.GetMessage(serviceProvider, notificationMessageGenerationInfo.Versions, ct);

        var attachments = await subscription.GetAttachments(serviceProvider, notificationMessageGenerationInfo.Versions).ToImmutableArrayAsync(ct);

        var mailMessage = new MailMessage
        {
            IsBodyHtml = true,
            From = subscription.Sender,
            Subject = subject,
            Body = body,
            Recipients = [.. notificationMessageGenerationInfo.Recipients],
            AttachmentList = [.. attachments]
        };

        if (subscription.InlineAttachments)
        {
            foreach (var attachment in mailMessage.Attachments)
            {
                InlineAttachmentHelper.InlineAttachment(mailMessage, attachment);
            }
        }

        return mailMessage;
    }

    private static IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>> ReGroup(
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> to,
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> copyTo,
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> replyTo) =>

        from g in new[] { to.GroupRecipients(RecipientRole.To), copyTo.GroupRecipients(RecipientRole.Copy), replyTo.GroupRecipients(RecipientRole.ReplyTo) }
                  .ToAsyncEnumerable().RegroupRecipients()

        let recipients = g.Select(pair => new NotificationRecipient(pair.Recipient, pair.Tag))

        select new NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>([.. recipients], g.Key);

    private async IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetAuthTo(
        DomainObjectVersions<TDomainObject> versions,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        if (subscription.SecurityRoles.Length > 0)
        {
            var notificationFilterGroups = await subscription.GetNotificationFilterGroups(serviceProvider, versions).ToImmutableArrayAsync(ct);

            if (notificationFilterGroups.Length > 0)
            {
                var emails = await employeeEmailExtractor.GetEmails(subscription.SecurityRoles, notificationFilterGroups).ToImmutableHashSetAsync(ct);

                if (emails.Count > 0)
                {
                    var renderingVersions = await versions.ChangeDomainObjectAsync(domainObject => subscription.ConvertToRenderingObject(serviceProvider, domainObject, ct));

                    yield return new NotificationMessageGenerationInfo<TRenderingObject>(emails, renderingVersions);
                }
            }
        }
    }

    private IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> GetMergeResult(
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> preTo,
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> authTo) =>

        from g in new[] { preTo.GroupRecipients(false), authTo.GroupRecipients(true) }.ToAsyncEnumerable().RegroupRecipients()

        let resultRecipients = g.Partial(
            pair => pair.Tag,
            (r1, r2) =>
                r1.Select<(string Recipient, bool Tag), string>(pair => pair.Recipient)
                  .GetEmailMergeResult(r2.Select<(string Recipient, bool Tag), string>(pair => pair.Recipient), subscription.RecipientMergeType))

        select new NotificationMessageGenerationInfo<TRenderingObject>([.. resultRecipients], g.Key);
}
