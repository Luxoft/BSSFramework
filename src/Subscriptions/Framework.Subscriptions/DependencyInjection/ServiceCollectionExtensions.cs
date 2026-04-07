using System.Net.Mail;

using Framework.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public void AddSubscriptions()
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
