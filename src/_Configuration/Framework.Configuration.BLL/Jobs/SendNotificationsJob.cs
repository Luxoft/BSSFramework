namespace Framework.Notification.Jobs;

public class SendNotificationsJob(IConfigurationBLLContext context, IExceptionStorage exceptionStorage = null)
    : ISendNotificationsJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = context.Logics.DomainObjectModification.Process();

        if (exceptionStorage != null)
        {
            result.Match(_ => { }, exceptionStorage.Save);
        }
    }
}
