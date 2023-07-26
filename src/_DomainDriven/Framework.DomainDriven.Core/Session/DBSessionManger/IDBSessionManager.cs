namespace Framework.DomainDriven;

public interface IDBSessionManager
{
    /// <summary>
    /// Попытка закрыть текущую бд-сессию (если она существует) и вызывать механизм сброса евентов в DALListener-ы
    /// </summary>
    Task TryCloseDbSessionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Попытка промаркировать текущую бд-сессию (если она существует), что случилась ошибка и ничего в базу записывать не нужно.
    /// </summary>
    void TryFaultDbSession();
}
