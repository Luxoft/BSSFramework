using Framework.Core;

namespace Framework.Events;

/// <summary>
/// Класс для отправки доменных евентов во внешнюю очередь (как правило MSMQ или Trace)
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TEventDTOBase"></typeparam>
public abstract class EventDTOMessageSender<TPersistentDomainObjectBase, TEventDTOBase> : EventDTOMessageSenderBase<TPersistentDomainObjectBase, TEventDTOBase>
        where TPersistentDomainObjectBase : class
{
    private readonly IMessageSender<TEventDTOBase> messageSender;

    protected EventDTOMessageSender(IMessageSender<TEventDTOBase> messageSender)
    {
        this.messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
    }

    public override void Send<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
    {
        this.messageSender.Send(this.ToEventDTOBase(domainObjectEventArgs));
    }
}
