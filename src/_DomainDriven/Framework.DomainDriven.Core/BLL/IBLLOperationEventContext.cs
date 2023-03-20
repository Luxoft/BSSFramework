namespace Framework.DomainDriven.BLL;

/// <summary>
/// Интерфейс объекта содержащего контейнер слушателей BLL-ных событий
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase">Тип доменного объекта</typeparam>
public interface IBLLOperationEventContext<in TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
{
    /// <summary>
    /// Возвращается контейнер отправителя BLL-ных событий
    /// </summary>
    IOperationEventSenderContainer<TPersistentDomainObjectBase> OperationSenders { get; }
}
