using System;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Контейнер для ручного инициирования евентов доменного объекта
    /// </summary>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TOperation">Тип операции</typeparam>
    public interface IForceEventContainer<in TDomainObject, in TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        /// <summary>
        /// Ручное инициирование события
        /// </summary>
        /// <param name="domainObject">Доменный объект</param>
        /// <param name="operation">Операция</param>
        void ForceEvent(TDomainObject domainObject, TOperation operation);
    }
}
