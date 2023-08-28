using Framework.Configuration.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.Notification;

namespace Framework.NotificationCore.Jobs;

public class SendNotificationsJob : ISendNotificationsJob
{
    private readonly IContextEvaluator<IConfigurationBLLContext> contextEvaluator;

    private readonly IExceptionStorage exceptionStorage;

    public SendNotificationsJob(
            IContextEvaluator<IConfigurationBLLContext> contextEvaluator,
            IExceptionStorage exceptionStorage = null)
    {
        this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
        this.exceptionStorage = exceptionStorage;
    }

    public void Send()
    {
        var result = this.contextEvaluator.Evaluate(
                                                    DBSessionMode.Write,
                                                    // todo: нужен рефакторинг - хотим разделить создание и отправку нотификаций, а то сейчас всё в кучу свалено
                                                    context => context.Logics.DomainObjectModification.Process());

        if (this.exceptionStorage != null)
        {
            result.Match(_ => { }, x => this.exceptionStorage.Save(x));
        }
    }
}
