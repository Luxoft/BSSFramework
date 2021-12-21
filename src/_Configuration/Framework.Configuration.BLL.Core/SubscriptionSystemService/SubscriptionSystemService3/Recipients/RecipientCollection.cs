using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients
{
    /// <summary>
    /// Коллекция получателей уведомлений по подписке.
    /// </summary>
    /// <seealso cref="IEnumerable{T}" />
    public sealed class RecipientCollection : IEnumerable<Recipient>
    {
        private static readonly RecipientComparer Comparer = new RecipientComparer();
        private readonly List<Recipient> store = new List<Recipient>();

        /// <summary>Создаёт экземпляр класса <see cref="RecipientCollection"/>.</summary>
        public RecipientCollection()
        {
        }

        /// <summary>Создаёт экземпляр класса <see cref="RecipientCollection"/>.</summary>
        /// <param name="recipients">
        ///     Коллекция получателей уведомлений, которая будет использована для инициализации создаваемого экземпляра.
        /// </param>
        /// <exception cref="ArgumentNullException">Аргумент recipients равен null.</exception>
        public RecipientCollection([NotNull] IEnumerable<Recipient> recipients)
        {
            if (recipients == null)
            {
                throw new ArgumentNullException(nameof(recipients));
            }

            foreach (var recipient in recipients.Where(recipient => !string.IsNullOrEmpty(recipient.Email)))
            {
                this.store.Add(recipient);
            }
        }

        /// <summary>Выполняет слияние двух коллекций получателей уведомлений.</summary>
        /// <param name="other">Коллекция получателей уведомлений, с которой необходимо произвести слияние.</param>
        /// <param name="mode">Тип слияния.</param>
        /// <returns>
        ///     Коллекция, которая является результатом слияния текущего экземпляра и коллекции,
        ///     переданной в аргументе other.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">Аргумент mode содержит недопустимое значение.</exception>
        public RecipientCollection Merge(RecipientCollection other, RecepientsMergeMode mode)
        {
            switch (mode)
            {
                case RecepientsMergeMode.Union:
                    return new RecipientCollection(this.Union(other, Comparer));
                case RecepientsMergeMode.Intersect:
                    return new RecipientCollection(this.Intersect(other, Comparer));
                case RecepientsMergeMode.LeftExceptRight:
                    return new RecipientCollection(this.Except(other, Comparer));
                case RecepientsMergeMode.RightExceptLeft:
                    return new RecipientCollection(other.Except(this, Comparer));
            }

            throw new ArgumentOutOfRangeException(nameof(mode));
        }

        /// <inheritdoc/>
        public IEnumerator<Recipient> GetEnumerator()
        {
            return this.store.GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        private class RecipientComparer : IEqualityComparer<Recipient>
        {
            public bool Equals(Recipient x, Recipient y)
            {
                return string.Equals(x.Email, y.Email, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Recipient obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
