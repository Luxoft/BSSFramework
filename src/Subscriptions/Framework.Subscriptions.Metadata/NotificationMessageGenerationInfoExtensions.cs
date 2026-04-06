using Framework.Subscriptions.Domain;

namespace Framework.Subscriptions.Metadata;

public static class NotificationMessageGenerationInfoExtensions
{
    public static IEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, (TRecipient recipient, TTag tag)>>
        GroupRecipients<TRenderingObject, TRecipient, TTag>(this IEnumerable<NotificationMessageGenerationInfo<TRenderingObject, TRecipient>> source, TTag tag)

        where TRenderingObject : class =>

        from item in source

        from recipient in item.Recipients

        group (recipient, tag) by item.Versions;


    public static IEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>
        RegroupRecipients<TRenderingObject, TValue>(
        this IEnumerable<IEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>> source)

        where TRenderingObject : class =>

        from groups in source

        from g in groups

        from recipient in g

        group recipient by g.Key;
}
