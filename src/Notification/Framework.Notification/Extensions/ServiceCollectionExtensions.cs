using Framework.Core;
using Framework.Notification.DTO;
using Framework.Notification.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddSmtpNotification(IConfiguration configuration)
        {
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.AddScoped<IMessageSender<NotificationEventDTO>, SmtpNotificationMessageSender>();
        }

        /// <summary>
        /// Дефолтный сервис по подмене получателей нотификаций. Работает на основе конфигурации RewriteReceiversSettings
        /// </summary>
        public void AddRewriteReceiversDependencies(IConfiguration configuration)
        {
            services.Configure<RewriteReceiversSettings>(configuration.GetSection("RewriteReceiversSettings"));

            services.AddSingleton<IRewriteReceiversService, RewriteReceiversService>();
        }
    }
}
