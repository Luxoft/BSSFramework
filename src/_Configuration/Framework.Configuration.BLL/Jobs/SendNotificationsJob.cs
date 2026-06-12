namespace Framework.Configuration.BLL.Jobs;

public class SendNotificationsJob(IConfigurationBLLContext context)
    : ISendNotificationsJob
{
    public Task ExecuteAsync(CancellationToken ct) => context.Logics.DomainObjectModification.ProcessAsync(1000, ct);
}
