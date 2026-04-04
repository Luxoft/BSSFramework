namespace Framework.Subscriptions.Subscriptions;

/// <summary>
/// Производит поиск Code first моделей подписок.
/// </summary>
public interface ISubscriptionMetadataFinder
{
    IEnumerable<ISubscriptionMetadata> Find();
}
