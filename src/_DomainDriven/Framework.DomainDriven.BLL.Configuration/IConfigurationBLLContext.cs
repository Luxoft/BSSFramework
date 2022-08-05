using System;

using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Configuration
{
    public interface IConfigurationBLLContext
    {
        bool DisplayInternalError { get; }

        IBLLSimpleQueryBase<IEmployee> GetEmployeeSource();

        /// <summary>
        /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
        /// </summary>
        /// <returns></returns>
        long GetCurrentRevision();
    }
}
