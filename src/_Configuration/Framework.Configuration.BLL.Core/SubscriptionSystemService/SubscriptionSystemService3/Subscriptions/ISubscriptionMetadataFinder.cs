using System.Collections.Generic;
using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions
{
    /// <summary>
    /// Производит поиск Code first моделей подписок.
    /// </summary>
    public interface ISubscriptionMetadataFinder
    {
        /// <summary>
        /// Производит поиск Code first моделей подписок. По умолчанию собираются подписки только с пустым конструктором
        /// </summary>
        /// <returns>Найденные модели подписок</returns>
        IEnumerable<ISubscriptionMetadata> Find();
    }
}
