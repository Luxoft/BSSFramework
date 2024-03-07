using Framework.Core;

namespace Framework.Events;

/// <summary>
/// Класс для отправки доменных евентов
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TEventDTOBase"></typeparam>
public abstract class
    EventDTOMessageSenderBase<TPersistentDomainObjectBase, TEventDTOBase> : IMessageSender<
    IDomainOperationSerializeData<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class
{
    public abstract void Send<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
        where TDomainObject : class, TPersistentDomainObjectBase;

    protected abstract TEventDTOBase ToEventDTOBase<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
        where TDomainObject : class, TPersistentDomainObjectBase;

    private void InternalSend<TDomainObject>(
        TDomainObject domainObject,
        EventOperation operation,
        object customSendObject)
        where TDomainObject : class, TPersistentDomainObjectBase

    {
        this.Send(
            new DomainOperationSerializeData<TDomainObject>
            {
                DomainObject = domainObject, Operation = operation, CustomSendObject = customSendObject
            });
    }

    void IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>>.Send(
        IDomainOperationSerializeData<TPersistentDomainObjectBase> domainObjectEventArgs)
    {
        if (domainObjectEventArgs == null) throw new ArgumentNullException(nameof(domainObjectEventArgs));

        var func = new Action<TPersistentDomainObjectBase, EventOperation, object>(this.InternalSend).CreateGenericMethod(
            domainObjectEventArgs.DomainObjectType,
            domainObjectEventArgs.Operation.GetType());

        func.Invoke(
            this,
            new object[] { domainObjectEventArgs.DomainObject, domainObjectEventArgs.Operation, domainObjectEventArgs.CustomSendObject });
    }
}
