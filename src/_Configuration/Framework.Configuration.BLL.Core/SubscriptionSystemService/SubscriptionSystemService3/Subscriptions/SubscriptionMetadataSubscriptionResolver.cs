using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Configuration.Core;
using Framework.Configuration.Domain;
using Framework.Configuration.SubscriptionModeling;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions
{
    /// <summary>
    ///     Компонент для поиска описаний подписок типа <see cref="ISubscriptionMetadata" />,
    ///     сохраненных в памяти и пребразования их в в экземпляры <see cref="Subscription" />.
    ///     Является декоратором экземпляра <see cref="SubscriptionResolver" />, выполняющего поиск подписок,
    ///     сохраненных в базе данных.
    /// </summary>
    /// <seealso cref="SubscriptionResolver" />
    public sealed class SubscriptionMetadataSubscriptionResolver : SubscriptionResolver
    {
        private readonly SubscriptionMetadataStore metadataStore;
        private readonly SubscriptionMetadataMapper metadataMapper;
        private readonly ConfigurationContextFacade configurationContextFacade;
        private readonly ILogger logger;

        /// <summary>
        ///     Создаёт экземпляр класса <see cref="SubscriptionMetadataSubscriptionResolver" />.
        /// </summary>
        /// <param name="resolver">Компонент, выполняющий поиск подписок.</param>
        /// <param name="metadataStore">Хранилище описаний подписок.</param>
        /// <param name="metadataMapper">Преобразователь экземпляров типа <see cref="ISubscriptionMetadata" /></param>
        /// <param name="configurationContextFacade"> Фасад контекста конфигурации.</param>
        /// <exception cref="ArgumentNullException">
        ///     Аргумент
        ///     <paramref name="resolver" />
        ///     или
        ///     <paramref name="metadataStore" />
        ///     или
        ///     <paramref name="metadataMapper" />
        ///     или
        ///     <paramref name="configurationContextFacade"/>
        ///     равен null.
        /// </exception>
        public SubscriptionMetadataSubscriptionResolver(
            [NotNull] SubscriptionMetadataStore metadataStore,
            [NotNull] SubscriptionMetadataMapper metadataMapper,
            [NotNull] ConfigurationContextFacade configurationContextFacade)
        {
            if (metadataStore == null)
            {
                throw new ArgumentNullException(nameof(metadataStore));
            }

            if (metadataMapper == null)
            {
                throw new ArgumentNullException(nameof(metadataMapper));
            }

            if (configurationContextFacade == null)
            {
                throw new ArgumentNullException(nameof(configurationContextFacade));
            }
            
            this.metadataStore = metadataStore;
            this.metadataMapper = metadataMapper;
            this.configurationContextFacade = configurationContextFacade;

            this.logger = Log.Logger.ForContext(this.GetType());
        }

        /// <inheritdoc />
        public override IEnumerable<Subscription> Resolve<T>([NotNull] DomainObjectVersions<T> versions)
        {
            if (versions == null)
            {
                throw new ArgumentNullException(nameof(versions));
            }

            var thisResult = this.GetSubscriptionsFromMetadata(versions.DomainObjectType);
            var result = thisResult;

            return result;
        }

        /// <inheritdoc />
        public override Subscription Resolve<T>(
            [NotNull] string subscriptionCode,
            [NotNull] DomainObjectVersions<T> versions)
        {
            if (subscriptionCode == null)
            {
                throw new ArgumentNullException(nameof(subscriptionCode));
            }

            if (versions == null)
            {
                throw new ArgumentNullException(nameof(versions));
            }

            var result = this
                .Resolve(versions)
                .FirstOrDefault(s => string.Equals(s.Code, subscriptionCode, StringComparison.OrdinalIgnoreCase));

            return result;
        }

        /// <inheritdoc />
        public override bool IsActiveSubscriptionForTypeExists([NotNull] Type domainObjectType)
        {
            if (domainObjectType == null)
            {
                throw new ArgumentNullException(nameof(domainObjectType));
            }

            var exists = this.IsSubscriptionExists(domainObjectType);

            return exists;
        }

        private bool IsSubscriptionExists(Type domainObjectType)
        {
            this.logger.Verbose("Check subscription metadata for domain object type '{domainObjectType}' exists", domainObjectType);

            var result = this.metadataStore.Get(domainObjectType).Any();

            this.logger.Verbose("Existing subscription metadata for domain object type '{domainObjectType}' {result}", domainObjectType, result ? "exists" : "not exists");

            return result;
        }

        private IEnumerable<Subscription> GetSubscriptionsFromMetadata(Type domainObjectType)
        {
            this.logger.Verbose("Search subscriptions in metadata for domain object type '{domainObjectType}'", domainObjectType);

            var metadata = this.metadataStore.Get(domainObjectType).ToList();

            if (!metadata.Any())
            {
                return Enumerable.Empty<Subscription>();
            }

            var activeCodeFirstSubscriptionCodes = this.configurationContextFacade.GetActiveCodeFirstSubscriptionCodes();

            var result = metadata
                .Where(m => activeCodeFirstSubscriptionCodes.Contains(m.Code))
                .Select(m => this.metadataMapper.Map(m)).ToArray();

            this.logger.Verbose("'{resultLength}' subscriptions has been found in metadata for domain object type '{domainObjectType}'", result.Length, domainObjectType);

            return result;
        }
    }
}
