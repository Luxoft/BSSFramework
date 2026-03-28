namespace Framework.Configuration.BLL;

using System.Collections.Generic;
using Framework.Notification.DTO;

public interface IAttachmentContainer
{
    List<NotificationAttachmentDTO> Attachments { get; set; }
}
