using System.Net.Mail;
using System.Runtime.CompilerServices;

using Anch.Core;
using Anch.IdentitySource;

using Framework.Core;
using Framework.Notification.Domain;
using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Subscriptions;

public class NotificationExtractor<TDomainObject, TRenderingObject>(
    IServiceProvider serviceProvider,
    IIdentityInfoSource identityInfoSource,
    INotificationEmailExtractor notificationEmailExtractor,
    ISubscription<TDomainObject, TRenderingObject> subscription,
    INotificationCommentSource notificationCommentSource,
    [FromKeyedServices(nameof(Subscriptions))]
    MailAddress? defaultSender = null) : INotificationExtractor<TDomainObject>
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

               let comment = notificationCommentSource.GetComment(mailMessage)

               let actualTechnicalInformation = comment == "" ? technicalInformation : technicalInformation with { Comment = comment }

               select new Notification.Domain.Notification(actualTechnicalInformation, mailMessage);
    }

    private async IAsyncEnumerable<MailMessage> GetMailMessages(
        DomainObjectVersions<TDomainObject> versions,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        if (await subscription.IsProcessed(serviceProvider, versions, ct))
        {
            var preTo = subscription.GetTo(serviceProvider, versions);
            var authTo = this.GetAuthTo(versions, ct);

            var resultTo = this.GetMergeResult(preTo, authTo);

            var actualResultTo = subscription.AllowEmptyListOfRecipients
                                     ? this.EnsureAtLeastOneTo(resultTo, versions, ct)
                                     : resultTo;

            var copyTo = subscription.GetCopyTo(serviceProvider, versions);
            var replyTo = subscription.GetReplyTo(serviceProvider, versions);

            await foreach (var mailMessage in this.SplitLetters(ReGroup(actualResultTo, copyTo, replyTo)).Select(this.ToMailMessage).WithCancellation(ct))
            {
                yield return mailMessage;
            }
        }
    }

    /// <summary>
    ///     Guarantees that for a subscription with <see cref="ISubscription.AllowEmptyListOfRecipients" /> == <c>true</c>
    ///     the stream of direct recipients (To) will contain at least one (empty) element even if none of the
    ///     the source of direct recipients (GetTo/roles) returned no addressee.
    /// </summary>
    /// <remarks>
    ///     Without this, if CopyTo/ReplyTo are also empty, the letter is not formed at all (grouping by
    ///     DomainObjectVersions simply does not get a key), and letter modifiers such as RedirectToSupportMailMessageModifier,
    ///     that substitute the support address into an empty To field do not get a chance to run.
    ///     The flag is deliberately applied only to direct recipients (To) - it does not apply to CopyTo/ReplyTo.
    /// </remarks>
    private async IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> EnsureAtLeastOneTo(
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject>> resultTo,
        DomainObjectVersions<TDomainObject> versions,
        [EnumeratorCancellation] CancellationToken ct = default)
    {
        await using var enumerator = resultTo.GetAsyncEnumerator(ct);

        if (await enumerator.MoveNextAsync())
        {
            yield return enumerator.Current;

            while (await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }
        else
        {
            var renderingVersions =
                await versions.ChangeDomainObjectAsync(domainObject => subscription.ConvertToRenderingObject(serviceProvider, domainObject, ct));

            yield return new NotificationMessageGenerationInfo<TRenderingObject>([], renderingVersions);
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
                              From = subscription.Sender ?? defaultSender ?? throw new InvalidOperationException("No sender specified."),
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

    /// <summary>
    ///     Splits a consolidated letter into individual ones if the subscription requires sending individual letters.
    /// </summary>
    /// <remarks>
    ///     Each delivery addressee (<see cref="RecipientRole.To" /> or <see cref="RecipientRole.Copy" />) gets its
    ///     its own letter, keeping its role. <see cref="RecipientRole.ReplyTo" /> addressees are not
    ///     delivery addressees, so they go into every formed letter.
    /// </remarks>
    private IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>> SplitLetters(
        IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>> source) =>

        subscription.SendIndividualLetters
            ? from info in source

              let replyTo = info.Recipients.Where(recipient => recipient.Role == RecipientRole.ReplyTo).ToList()

              from deliveryRecipient in info.Recipients.Where(recipient => recipient.Role != RecipientRole.ReplyTo)

              select new NotificationMessageGenerationInfo<TRenderingObject, NotificationRecipient>([deliveryRecipient, .. replyTo], info.Versions)
            : source;

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
        if (subscription.SecurityRule != null)
        {
            var notificationFilterGroups = await subscription.GetNotificationFilterGroups(serviceProvider, versions).ToImmutableArrayAsync(ct);

            var emails = await notificationEmailExtractor.GetEmails(subscription.SecurityRule, notificationFilterGroups).ToImmutableHashSetAsync(ct);

            var renderingVersions =
                await versions.ChangeDomainObjectAsync(domainObject => subscription.ConvertToRenderingObject(serviceProvider, domainObject, ct));

            yield return new NotificationMessageGenerationInfo<TRenderingObject>(emails, renderingVersions);
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
