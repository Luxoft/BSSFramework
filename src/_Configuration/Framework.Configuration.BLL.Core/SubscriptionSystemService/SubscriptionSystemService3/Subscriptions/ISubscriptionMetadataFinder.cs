using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <summary>
/// Производит поиск Code first моделей подписок.
/// </summary>
public interface ISubscriptionMetadataFinder
{
    IEnumerable<ISubscriptionMetadata> Find();
}
