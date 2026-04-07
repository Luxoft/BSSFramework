using System.Net.Mail;

using Framework.Core;
using Framework.Notification.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddSmtpNotification(IConfiguration configuration, bool isProd)
        {
            services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            services.AddSingleton<ISubjectCleaner, SubjectCleaner>();

            services.AddSingleton<IRewriteReceiversService, RewriteReceiversService>();

            if (isProd)
            {
                services.AddScoped<IMessageSender<MailMessage>, ProdSmtpMessageSender>();
            }
            else
            {
                services.AddScoped<IMessageSender<MailMessage>, TestSmtpMessageSender>();
            }

            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<RewriteReceiversSettings>(configuration.GetSection(nameof(RewriteReceiversSettings)));
        }
    }
}
