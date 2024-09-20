using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.Notification;

namespace Framework.NotificationCore.Jobs;

public class SendNotificationsJob(
    IServiceEvaluator<IConfigurationBLLContext> contextEvaluator,
    IExceptionStorage exceptionStorage = null)
    : ISendNotificationsJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var result = contextEvaluator.Evaluate(
            DBSessionMode.Write,
            // todo: нужен рефакторинг - хотим разделить создание и отправку нотификаций, а то сейчас всё в кучу свалено
            context => context.Logics.DomainObjectModification.Process());

        if (exceptionStorage != null)
        {
            result.Match(_ => { }, x => exceptionStorage.Save(x));
        }
    }
}
