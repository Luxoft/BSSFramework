using Framework.Core.MessageSender;
using Framework.Notification.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.Extensions;

public static class NotificationSenderExtensions
{
    public static void AddSmtpNotification(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

        services.AddScoped<IMessageSender<Notification.DTO.NotificationEventDTO>, SmtpNotificationMessageSender>();
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
