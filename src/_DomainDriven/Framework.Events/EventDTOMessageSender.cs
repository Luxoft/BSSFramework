using Framework.Core;

namespace Framework.Events;

/// <summary>
/// Класс для отправки доменных евентов во внешнюю очередь (как правило MSMQ или Trace)
/// </summary>
/// <typeparam name="TBLLContext"></typeparam>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TEventDTOBase"></typeparam>
public abstract class EventDTOMessageSender<TBLLContext, TPersistentDomainObjectBase, TEventDTOBase> : EventDTOMessageSenderBase<TBLLContext, TPersistentDomainObjectBase, TEventDTOBase>
        where TPersistentDomainObjectBase : class
        where TBLLContext : class
{
    private readonly IMessageSender<TEventDTOBase> messageSender;

    protected EventDTOMessageSender(TBLLContext context, IMessageSender<TEventDTOBase> messageSender)
            : base(context)
    {
        this.messageSender = messageSender ?? throw new ArgumentNullException(nameof(messageSender));
    }

    public override void Send<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
    {
        this.messageSender.Send(this.ToEventDTOBase(domainObjectEventArgs));
    }
}
