using Framework.Application.Events;

namespace Framework.BLL;

public interface IBLLOperationEventContext
{
    IEventOperationSender OperationSender { get; }
}
