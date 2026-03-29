using Framework.Notification.DTO;

namespace Framework.Configuration.BLL.Notification;

public interface IAttachmentContainer
{
    List<NotificationAttachmentDTO> Attachments { get; set; }
}
