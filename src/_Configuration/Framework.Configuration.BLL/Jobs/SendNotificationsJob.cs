using Framework.Core;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Framework.Notification.Jobs;

public class SendNotificationsJob(IConfigurationBLLContext context)
    : ISendNotificationsJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        context.Logics.DomainObjectModification.Process();
    }
}
