using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.Events;

/// <summary>
/// Класс для отправки доменных евентов
/// </summary>
/// <typeparam name="TBLLContext"></typeparam>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TEventDTOBase"></typeparam>
public abstract class EventDTOMessageSenderBase<TBLLContext, TPersistentDomainObjectBase, TEventDTOBase> : BLLContextContainer<TBLLContext>, IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>>
        where TPersistentDomainObjectBase : class
        where TBLLContext : class
{
    protected EventDTOMessageSenderBase(TBLLContext context)
            : base(context)
    {
    }

    public abstract void Send<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum;

    protected abstract TEventDTOBase ToEventDTOBase<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum;

    private void InternalSend<TDomainObject, TOperation>(
            TDomainObject domainObject,
            TOperation operation,
            object customSendObject)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
    {
        this.Send(new DomainOperationSerializeData<TDomainObject, TOperation> { DomainObject = domainObject, Operation = operation, CustomSendObject = customSendObject});
    }

    void IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>>.Send(IDomainOperationSerializeData<TPersistentDomainObjectBase> domainObjectEventArgs)
    {
        if (domainObjectEventArgs == null) throw new ArgumentNullException(nameof(domainObjectEventArgs));

        var func = new Action<TPersistentDomainObjectBase, TypeCode, object>(this.InternalSend).CreateGenericMethod(domainObjectEventArgs.DomainObjectType, domainObjectEventArgs.Operation.GetType());

        func.Invoke(this, new object[] { domainObjectEventArgs.DomainObject, domainObjectEventArgs.Operation, domainObjectEventArgs.CustomSendObject });
    }
}
