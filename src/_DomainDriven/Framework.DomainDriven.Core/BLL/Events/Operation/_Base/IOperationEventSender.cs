using System;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Контейнер для ручного инициирования евентов доменного объекта
    /// </summary>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TOperation">Тип операции</typeparam>
    public interface IOperationEventSender<in TDomainObject, in TOperation>
        where TDomainObject : class
        where TOperation : struct, Enum
    {
        void SendEvent(IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs);
    }
}
