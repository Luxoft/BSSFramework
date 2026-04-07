using System.Net.Mail;

using Framework.Core;
using Framework.Notification.MailMessageModifier;
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
            services.AddSingleton<IMailMessageModifier, HtmlMarkerMessageModifier>();
            services.AddSingleton<IMailMessageModifier, SubjectCleanerMailMessageModifier>();
            services.AddSingleton<IMailMessageModifier, RedirectToSupportMailMessageModifier>();

            if (!isProd)
            {
                services.AddSingleton<IMailMessageModifier, RedirectToTestAddress>();
                services.AddSingleton<IMailMessageModifier, RewriteReceiversMailMessageModifier>();
            }

            services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
            services.AddSingleton<IMessageSender<MailMessage>, SmtpMessageSender>();

            services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
            services.Configure<RewriteReceiversSettings>(configuration.GetSection(nameof(RewriteReceiversSettings)));
        }
    }
}
