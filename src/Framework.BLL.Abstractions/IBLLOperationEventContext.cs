using Framework.Events;

namespace Framework.BLL;

public interface IBLLOperationEventContext
{
    IEventOperationSender OperationSender { get; }
}
