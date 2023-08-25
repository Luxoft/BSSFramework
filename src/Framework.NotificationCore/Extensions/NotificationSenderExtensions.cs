using Framework.Core;
using Framework.NotificationCore.Jobs;
using Framework.NotificationCore.Senders;
using Framework.NotificationCore.Services;
using Framework.NotificationCore.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.DependencyInjection;

public static class NotificationSenderExtensions
{
    public static void RegisterNotificationJob(this IServiceCollection services)
    {
        services.AddScoped<ISendNotificationsJob, SendNotificationsJob>();
    }

    public static void RegisterNotificationSmtp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        services.ReplaceScoped<IMessageSender<Framework.Notification.DTO.NotificationEventDTO>, SmtpNotificationMessageSender>();
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
