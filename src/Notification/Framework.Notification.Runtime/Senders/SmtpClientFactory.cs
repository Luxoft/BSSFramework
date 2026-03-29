using System.Net.Mail;

using Framework.Notification.Settings;

using Microsoft.Extensions.Options;

namespace Framework.Notification.Senders;

public class SmtpClientFactory(IOptions<SmtpSettings> settings) : ISmtpClientFactory
{
    public SmtpClient CreateSmtpClient() =>

        settings.Value.SmtpEnabled
            ? new SmtpClient(settings.Value.Server, settings.Value.Port) { UseDefaultCredentials = true }
            : new SmtpClient
              {
                  UseDefaultCredentials = true,
                  DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                  PickupDirectoryLocation = settings.Value.OutputFolder
              };
}
