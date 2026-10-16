using System.Net.Mail;

using Framework.Core;
using Framework.Infrastructure.DependencyInjection;
using Framework.Notification.MailMessageModifier;
using Framework.Notification.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.DependencyInjection;

public static class ServiceCollectionExtensions
{
    extension<TSelf>(IBssFrameworkSetup<TSelf> setup)
        where TSelf : IBssFrameworkSetup<TSelf>
    {
        public TSelf AddNotification(IConfiguration configuration, bool isProd) =>
            setup.AddServices(sc => sc.AddNotification(configuration, isProd));
    }

    extension(IServiceCollection services)
    {
        public void AddNotification(IConfiguration configuration, bool isProd)
        {
            services.AddScoped<IMessageSender<Notification.Domain.Notification>, RootNotificationMessageSender>();
            services.AddScoped<ISentNotificationLogger, RootSentNotificationLogger>();

            services.AddSingleton<IActualMailMessageProcessor, ActualMailMessageProcessor>();
            services.AddSingleton<IActualNotificationProcessor, ActualNotificationProcessor>();

            services.AddSingleton<IMailMessageModifier, HtmlMarkerMessageModifier>();
            services.AddSingleton<IMailMessageModifier, SubjectCleanerMailMessageModifier>();
            services.AddSingleton<IMailMessageModifier, RedirectToSupportMailMessageModifier>();

            services.AddKeyedSingleton<IMailMessageModifier, GdprMailMessageModifier>(IMailMessageModifier.LoggerKey);

            if (!isProd)
            {
                services.AddSingleton<IMailMessageModifier, RedirectToTestAddress>();
                services.AddSingleton<IMailMessageModifier, RewriteReceiversMailMessageModifier>();
            }

            services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            services.AddKeyedSingleton<IMessageSender<Notification.Domain.Notification>, SmtpNotificationMessageSender>(RootNotificationMessageSender.InnerKey);
            services.AddSingleton<IMessageSender<MailMessage>, SmtpMessageSender>();

            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<RewriteReceiversSettings>(configuration.GetSection(nameof(RewriteReceiversSettings)));
        }
    }
}
