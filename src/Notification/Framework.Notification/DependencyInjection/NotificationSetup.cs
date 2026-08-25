using Anch.DependencyInjection;

using Framework.Core;
using Framework.Notification.MailMessageModifier;
using Framework.Notification.Settings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Notification.DependencyInjection;

public class NotificationSetup(IConfiguration configuration) : INotificationSetup, IServiceInitializer
{
    private bool isProd = true;

    private Type senderType = typeof(SmtpMessageSender);

    public INotificationSetup SetSender<TSender>()
        where TSender : class, IMessageSender<Domain.Notification>
    {
        this.senderType = typeof(TSender);

        return this;
    }

    public INotificationSetup IsProduction(bool value)
    {
        this.isProd = value;

        return this;
    }

    public void Initialize(IServiceCollection services)
    {
        services.AddSingleton<IMessageSender<Notification.Domain.Notification>, RootNotificationMessageSender>();

        services.AddSingleton<IMailMessageModifier, HtmlMarkerMessageModifier>();
        services.AddSingleton<IMailMessageModifier, SubjectCleanerMailMessageModifier>();
        services.AddSingleton<IMailMessageModifier, RedirectToSupportMailMessageModifier>();

        if (!this.isProd)
        {
            services.AddSingleton<IMailMessageModifier, RedirectToTestAddress>();
            services.AddSingleton<IMailMessageModifier, RewriteReceiversMailMessageModifier>();
        }

        services.AddKeyedSingleton<IMailMessageModifier, GdprCleanerModifier>(IMailMessageModifier.LoggerKey);

        services.AddSingleton<ISmtpClientFactory, SmtpClientFactory>();
        services.AddKeyedSingleton(typeof(IMessageSender<Notification.Domain.Notification>), "Native", this.senderType);

        services.Configure<SmtpSettings>(configuration.GetSection(nameof(SmtpSettings)));
        services.Configure<RewriteReceiversSettings>(configuration.GetSection(nameof(RewriteReceiversSettings)));
    }
}
