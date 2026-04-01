using System.Collections;

namespace Framework.Configuration.BLL.SubscriptionSystemService.SubscriptionSystemService3.Recipients;

/// <summary>
/// Коллекция получателей уведомлений по подписке.
/// </summary>
/// <seealso cref="IEnumerable{T}" />
public sealed class RecipientCollection : IEnumerable<Recipient>
{
    private static readonly RecipientComparer Comparer = new();
    private readonly List<Recipient> store = [];

    /// <summary>Создаёт экземпляр класса <see cref="RecipientCollection"/>.</summary>
    public RecipientCollection()
    {
    }

    /// <summary>Создаёт экземпляр класса <see cref="RecipientCollection"/>.</summary>
    /// <param name="recipients">
    ///     Коллекция получателей уведомлений, которая будет использована для инициализации создаваемого экземпляра.
    /// </param>
    /// <exception cref="ArgumentNullException">Аргумент recipients равен null.</exception>
    public RecipientCollection(IEnumerable<Recipient> recipients)
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
    public RecipientCollection Merge(RecipientCollection other, RecipientsMergeMode mode)
    {
        switch (mode)
        {
            case RecipientsMergeMode.Union:
                return new RecipientCollection(this.Union(other, Comparer));
            case RecipientsMergeMode.Intersect:
                return new RecipientCollection(this.Intersect(other, Comparer));
            case RecipientsMergeMode.LeftExceptRight:
                return new RecipientCollection(this.Except(other, Comparer));
            case RecipientsMergeMode.RightExceptLeft:
                return new RecipientCollection(other.Except(this, Comparer));
        }

        throw new ArgumentOutOfRangeException(nameof(mode));
    }

    /// <inheritdoc/>
    public IEnumerator<Recipient> GetEnumerator() => this.store.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    private class RecipientComparer : IEqualityComparer<Recipient>
    {
        public bool Equals(Recipient x, Recipient y) => string.Equals(x.Email, y.Email, StringComparison.OrdinalIgnoreCase);

        public int GetHashCode(Recipient obj) => obj.GetHashCode();
    }
}
