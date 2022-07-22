namespace Framework.DomainDriven.BLL;

public interface ICurrentRevisionService
{

    /// <summary>
    /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();
}
