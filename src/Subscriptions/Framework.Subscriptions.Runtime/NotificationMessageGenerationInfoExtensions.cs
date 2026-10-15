using System.Collections;

using Framework.Subscriptions.Domain;
using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions;

public static class NotificationMessageGenerationInfoExtensions
{
    /// <remarks>
    ///     Группировка ведётся по самому <paramref name="source" />-элементу (а не по его получателям), чтобы
    ///     элемент с пустым списком получателей (см. <see cref="ISubscription.AllowEmptyListOfRecipients" />)
    ///     не терялся, а порождал группу без элементов вместо того, чтобы бесследно исчезнуть при распрямлении.
    /// </remarks>
    public static IAsyncEnumerable<IGrouping<DomainObjectVersions<TRenderingObject>, (TRecipient Recipient, TTag Tag)>>
        GroupRecipients<TRenderingObject, TRecipient, TTag>(this IAsyncEnumerable<NotificationMessageGenerationInfo<TRenderingObject, TRecipient>> source, TTag tag)

        where TRenderingObject : class =>

        from item in source

        group item by item.Versions into g

        select CreateGrouping(g.Key, g.SelectMany(item => item.Recipients.Select(recipient => (recipient, tag))));

    /// <remarks>
    ///     Группировка ведётся по ключу уже построенных группировок (а не по их элементам), чтобы пустая
    ///     группировка (см. <see cref="GroupRecipients{TRenderingObject,TRecipient,TTag}" />) не терялась при
    ///     объединении потоков To/CopyTo/ReplyTo, а сохраняла свой ключ версии с пустым набором элементов.
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
