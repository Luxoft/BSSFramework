namespace Framework.Subscriptions.Domain;

/// <summary>
/// Интерфейс контейнера для электронных адресов пользователей
/// </summary>
public interface ICurrentUserEmailContainer
{
    string CurrentUserEmail { get; }
}
