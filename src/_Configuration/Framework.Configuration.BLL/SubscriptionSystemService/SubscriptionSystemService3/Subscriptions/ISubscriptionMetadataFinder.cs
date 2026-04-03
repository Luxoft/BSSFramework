using Framework.Subscriptions;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Subscriptions;

/// <summary>
/// Производит поиск Code first моделей подписок.
/// </summary>
public interface ISubscriptionMetadataFinder
{
    IEnumerable<ISubscriptionMetadata> Find();
}
