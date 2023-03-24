using System.Net.Mail;

namespace Framework.Notification;

public class Notification
{
    public Notification(NotificationTechnicalInformation technicalInformation, MailMessage message)
    {
        this.TechnicalInformation = technicalInformation;
        this.Message = message;
    }

    public NotificationTechnicalInformation TechnicalInformation
    {
        get;
    }

    public MailMessage Message
    {
        get;
    }
}
