namespace Framework.DomainDriven.BLL.Configuration;

public interface IConfigurationBLLContext
{
    IEmployeeSource EmployeeSource { get; }

    /// <summary>
    /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
    /// </summary>
    /// <returns></returns>
    long GetCurrentRevision();
}
