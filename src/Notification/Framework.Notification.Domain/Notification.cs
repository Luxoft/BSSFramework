using System.Net.Mail;

namespace Framework.Notification.Domain;

public record Notification(NotificationTechnicalInformation TechnicalInformation, MailMessage Message);
