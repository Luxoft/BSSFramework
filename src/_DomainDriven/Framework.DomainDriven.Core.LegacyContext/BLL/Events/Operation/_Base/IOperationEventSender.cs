namespace Framework.DomainDriven.BLL;

/// <summary>
/// Контейнер для ручного инициирования евентов доменного объекта
/// </summary>
/// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
public interface IOperationEventSender<in TDomainObject>
    where TDomainObject : class
{
    void SendEvent(IDomainOperationEventArgs<TDomainObject> eventArgs);
}
