using Framework.Core;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.ServiceModel;
using Framework.Notification.DTO;
using Framework.NotificationCore.Jobs;
using Framework.NotificationCore.Senders;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using IConfigurationBLLContext = Framework.Configuration.BLL.IConfigurationBLLContext;

namespace Framework.DependencyInjection
{
    public static class NotificationSenderExtensions
    {
        public static void RegisterMessageSenderDependencies<TBLLContext>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TBLLContext : IConfigurationBLLContextContainer<IConfigurationBLLContext>
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<ISendNotificationsJob, SendNotificationsJob<TBLLContext>>();
            services.AddSingleton<IMessageSender<NotificationEventDTO>, EmptyMessageSender>();
        }

        /// <summary>
        /// Дефолтный сервис по подмене получателей нотификаций. Работает на основе конфигурации RewriteReceiversSettings
        /// </summary>
        public static void RegisterRewriteReceiversDependencies(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RewriteReceiversSettings>(configuration.GetSection("RewriteReceiversSettings"));
            services.AddSingleton<IRewriteReceiversService, RewriteReceiversService>();
        }
    }
}
