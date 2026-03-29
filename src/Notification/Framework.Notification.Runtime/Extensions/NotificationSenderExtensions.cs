using Framework.Core.MessageSender;
using Framework.Notification.Jobs;
using Framework.Notification.Senders;
using Framework.Notification.Services;
using Framework.Notification.Settings;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.Extensions;

public static class NotificationSenderExtensions
{
    public static void RegisterNotificationJob(this IServiceCollection services)
    {
        services.AddScoped<ISendNotificationsJob, SendNotificationsJob>();
    }

    public static void RegisterNotificationSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.ReplaceScoped<IMessageSender<Notification.DTO.NotificationEventDTO>, SmtpNotificationMessageSender>();
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
