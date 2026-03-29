namespace Framework.Configuration.BLL.Jobs;

public class SendNotificationsJob(IConfigurationBLLContext context)
    : ISendNotificationsJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        context.Logics.DomainObjectModification.Process();
    }
}
