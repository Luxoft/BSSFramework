using Framework.Notification;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Recipients;

/// <summary>
/// Получатель уведомления по подписке.
/// </summary>
/// <seealso cref="IEmployee" />
public sealed class Recipient : IEmployee
{
    /// <summary>
    /// Создаёт экземпляр класса <see cref="Recipient"/>.
    /// </summary>
    /// <param name="login">Имя учётной записи получателя уведомления.</param>
    /// <param name="email">Адрес электронной почты получателя уведомления.</param>
    public Recipient(string login, string email)
    {
        this.Login = login;
        this.Email = email;
    }

    /// <summary>Возвращает имя учётной записи получателя уведомления.</summary>
    /// <value>Имя учётной записи получателя уведомления.</value>
    public string Login { get; }

    /// <summary>Возвращает адрес электронной почты получателя уведомления.</summary>
    /// <value>Адрес электронной почты получателя уведомления.</value>
    public string Email { get; }
}
