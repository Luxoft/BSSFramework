using Framework.Application.Events;
using Framework.Events;

namespace Framework.BLL;

public interface IBLLOperationEventContext
{
    IEventOperationSender OperationSender { get; }
}
