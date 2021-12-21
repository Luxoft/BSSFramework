using System;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients
{
    /// <summary>
    /// Контейнер списков получателей уведомлений по подписке.
    /// </summary>
    public sealed class RecipientsBag
    {
        /// <summary>
        /// Создаёт экземпляр класса <see cref="RecipientsBag" />.
        /// </summary>
        /// <param name="to">Получатели, которые будут указаны в поле To.</param>
        /// <param name="cc">Получатели, которые будут указаны в поле Cc.</param>
        /// <param name="replyTo">The reply to.</param>
        /// <exception cref="ArgumentNullException">Аргумент
        /// to
        /// или
        /// cc равен null.</exception>
        public RecipientsBag([NotNull] RecipientCollection to, [NotNull] RecipientCollection cc, [NotNull] RecipientCollection replyTo)
        {
            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            if (cc == null)
            {
                throw new ArgumentNullException(nameof(cc));
            }

            this.To = to;
            this.Cc = cc;
            this.ReplyTo = replyTo ?? throw new ArgumentNullException(nameof(replyTo));
        }

        /// <summary>Возвращает список получателей, которые будут указаны в поле To.</summary>
        /// <value>Список получателей, которые будут указаны в поле To.</value>
        public RecipientCollection To { get; }

        /// <summary>Возвращает список получателей, которые будут указаны в поле Cc.</summary>
        /// <value>Список получателей, которые будут указаны в поле Cc.</value>
        public RecipientCollection Cc { get; }

        public RecipientCollection ReplyTo { get; }
    }
}
