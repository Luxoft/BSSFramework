using Framework.Events;

namespace Framework.DomainDriven.BLL;

public interface IBLLOperationEventContext
{
    IEventOperationSender OperationSender { get; }
}
