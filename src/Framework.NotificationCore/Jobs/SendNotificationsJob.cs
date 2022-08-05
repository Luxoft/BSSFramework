using System;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;
using Framework.Notification;

using JetBrains.Annotations;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Framework.NotificationCore.Jobs
{
    public class SendNotificationsJob<TBLLContext> : ISendNotificationsJob
        where TBLLContext: IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IContextEvaluator<TBLLContext> contextEvaluator;

        private readonly IExceptionStorage exceptionStorage;

        public SendNotificationsJob(
            [NotNull] IContextEvaluator<TBLLContext> contextEvaluator,
            [NotNull] IExceptionStorage exceptionStorage = null)
        {
            this.contextEvaluator = contextEvaluator ?? throw new ArgumentNullException(nameof(contextEvaluator));
            this.exceptionStorage = exceptionStorage;
            }

        public void Send()
        {
            var result = this.contextEvaluator.Evaluate(
                DBSessionMode.Write,
                // todo: нужен рефакторинг - хотим разделить создание и отправку нотификаций, а то сейчас всё в кучу свалено
                z =>
                {
                    return z.Configuration.Logics.DomainObjectModification.Process();
                });

            if (this.exceptionStorage != null)
            {
                result.Match(_ => { }, x => this.exceptionStorage.Save(x));
            }
        }
    }
}
