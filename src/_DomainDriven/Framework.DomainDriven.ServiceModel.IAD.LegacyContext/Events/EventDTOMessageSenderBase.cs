using CommonFramework;

using Framework.Core;
using Framework.Events;
using Framework.Events.Legacy;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Класс для отправки доменных евентов
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public abstract class EventDTOMessageSenderBase<TPersistentDomainObjectBase> : IEventDTOMessageSender<TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class
{
    public abstract Task SendAsync<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs, CancellationToken cancellationToken)
        where TDomainObject : class, TPersistentDomainObjectBase;

    private async Task InternalSend<TDomainObject>(
        TDomainObject domainObject,
        EventOperation operation,
        object? customSendObject,
        CancellationToken cancellationToken)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        await this.SendAsync(
            new DomainOperationSerializeData<TDomainObject>
            {
                DomainObject = domainObject,
                Operation = operation,
                CustomSendObject = customSendObject
            }, cancellationToken);
    }

    async Task IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>>.SendAsync(
        IDomainOperationSerializeData<TPersistentDomainObjectBase> domainObjectEventArgs,
        CancellationToken cancellationToken)
    {
        if (domainObjectEventArgs == null) throw new ArgumentNullException(nameof(domainObjectEventArgs));

        var func = new Func<TPersistentDomainObjectBase, EventOperation, object?, CancellationToken, Task>(this.InternalSend).CreateGenericMethod(
            domainObjectEventArgs.DomainObjectType);

        await func.Invoke<Task>(
            this,
            [domainObjectEventArgs.DomainObject, domainObjectEventArgs.Operation, domainObjectEventArgs.CustomSendObject, cancellationToken]);
    }
}
