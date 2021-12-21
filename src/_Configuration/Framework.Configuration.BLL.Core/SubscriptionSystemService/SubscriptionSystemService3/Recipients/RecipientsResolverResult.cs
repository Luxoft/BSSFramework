using System;

using Framework.Configuration.Core;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients
{
    /// <summary>
    ///     Результат вызова <see cref="RecipientsResolver.Resolve{T}" />
    /// </summary>
    public class RecipientsResolverResult
    {
        /// <summary>Создаёт экземпляр класса <see cref="RecipientsResolverResult" />.</summary>
        /// <param name="recipientsBag">Контейнер списков получателей уведомлений по подписке.</param>
        /// <param name="domainObjectVersions">Версии доменного объекта. Обновляются после вызова Generation лямбды.</param>
        /// <exception cref="ArgumentNullException">
        ///     Аргумент
        ///     <paramref name="recipientsBag" />
        ///     или
        ///     <paramref name="domainObjectVersions" /> равен null.
        /// </exception>
        public RecipientsResolverResult(
            [NotNull] RecipientsBag recipientsBag,
            [NotNull] DomainObjectVersions<object> domainObjectVersions)
        {
            if (recipientsBag == null)
            {
                throw new ArgumentNullException(nameof(recipientsBag));
            }

            if (domainObjectVersions == null)
            {
                throw new ArgumentNullException(nameof(domainObjectVersions));
            }

            this.RecipientsBag = recipientsBag;
            this.DomainObjectVersions = domainObjectVersions;
        }

        /// <summary>Возвращает контейнер списков получателей уведомлений по подписке.</summary>
        /// <value>Контейнер списков получателей уведомлений по подписке.</value>
        public RecipientsBag RecipientsBag { get; }

        /// <summary>Возвращает версии доменного объекта.</summary>
        /// <value>Версии доменного объекта.</value>
        public DomainObjectVersions<object> DomainObjectVersions { get; }
    }
}
