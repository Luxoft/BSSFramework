using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public static class NotificationMessageGenerationInfoExtensions
{
    public static IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, (TRecipient Recipient, TTag Tag)>>
        GroupRecipients<TRenderingObject, TRecipient, TTag>(this IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, TRecipient>> source, TTag tag)

        where TRenderingObject : class =>

        from item in source

        from recipient in item.Recipients

        group (recipient, tag) by item.Versions;


    public static IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>
        RegroupRecipients<TRenderingObject, TValue>(
        this IAsyncEnumerable<IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>> source)

        where TRenderingObject : class =>

        from groups in source

        from g in groups

        from recipient in g

        group recipient by g.Key;
}

