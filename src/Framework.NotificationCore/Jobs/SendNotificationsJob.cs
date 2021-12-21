using System;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Framework.NotificationCore.Jobs
{
    public class SendNotificationsJob<TBLLContext> : ISendNotificationsJob
        where TBLLContext: IConfigurationBLLContextContainer<IConfigurationBLLContext>
    {
        private readonly IServiceEnvironment<TBLLContext> serviceEnvironment;

        private readonly IExceptionProcessor exceptionProcessor;

        private readonly IServiceProvider serviceProvider;

        public SendNotificationsJob(
            [NotNull] IServiceEnvironment<TBLLContext> serviceEnvironment,
            [NotNull] IExceptionProcessor exceptionProcessor,
            [NotNull] IServiceProvider serviceProvider)
        {
            this.serviceEnvironment = serviceEnvironment ?? throw new ArgumentNullException(nameof(serviceEnvironment));
            this.exceptionProcessor = exceptionProcessor ?? throw new ArgumentNullException(nameof(exceptionProcessor));
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public void Send()
        {
            var result = this.serviceEnvironment.GetContextEvaluator(this.serviceProvider).Evaluate(
                DBSessionMode.Write,
                // todo: нужен рефакторинг - хотим разделить создание и отправку нотификаций, а то сейчас всё в кучу свалено
                z =>
                {
                    return z.Configuration.Logics.DomainObjectModification.Process();
                });

            result.Match(_ => { }, x => this.exceptionProcessor.Process(x));
        }
    }
}
