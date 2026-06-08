namespace Framework.Database;

public interface IDBSessionManager
{
    /// <summary>
    /// Попытка закрыть текущую бд-сессию (если она существует) и вызывать механизм сброса евентов в DALListener-ы
    /// </summary>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task TryCloseDbSessionAsync(CancellationToken ct);

    /// <summary>
    ///  Попытка промаркировать текущую бд-сессию (если она существует), что случилась ошибка и ничего в базу записывать не нужно.
    /// </summary>
    void TryFaultDbSession();
}