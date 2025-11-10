using System.Net.Mail;

namespace Framework.Notification;

public record Notification(NotificationTechnicalInformation TechnicalInformation, MailMessage Message);
