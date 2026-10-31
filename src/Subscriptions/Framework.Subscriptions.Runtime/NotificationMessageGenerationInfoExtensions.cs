using System.Collections;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public static class NotificationMessageGenerationInfoExtensions
{
    /// <remarks>
    ///     Grouping is done by the <paramref name="source" /> element itself (not by its recipients) so that
    ///     an element with an empty list of recipients (see <see cref="ISubscription.AllowEmptyListOfRecipients" />)
    ///     is not lost but produces a group without elements instead of vanishing without a trace on flattening.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, (TRecipient Recipient, TTag Tag)>>
        GroupRecipients<TRenderingObject, TRecipient, TTag>(this IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, TRecipient>> source, TTag tag)

        where TRenderingObject : class =>

        from item in source

        group item by item.Versions into g

        select CreateGrouping(g.Key, g.SelectMany(item => item.Recipients.Select(recipient => (recipient, tag))));

    /// <remarks>
    ///     Grouping is done by the key of the already built groupings (not by their elements) so that an empty
    ///     grouping (see <see cref="GroupRecipients{TRenderingObject,TRecipient,TTag}" />) is not lost during
    ///     merging of the To/CopyTo/ReplyTo streams, and kept its version key with an empty set of elements.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>
        RegroupRecipients<TRenderingObject, TValue>(
        this IAsyncEnumerable<IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, TValue>>> source)

        where TRenderingObject : class =>

        from groups in source

        from g in groups

        group g by g.Key into keyed

        select CreateGrouping(keyed.Key, keyed.SelectMany(g => g));

    private static IGrouping<TKey, TValue> CreateGrouping<TKey, TValue>(TKey key, IEnumerable<TValue> values) => new Grouping<TKey, TValue>(key, values);

    private sealed class Grouping<TKey, TValue>(TKey key, IEnumerable<TValue> values) : IGrouping<TKey, TValue>
    {
        public TKey Key { get; } = key;

        public IEnumerator<TValue> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
