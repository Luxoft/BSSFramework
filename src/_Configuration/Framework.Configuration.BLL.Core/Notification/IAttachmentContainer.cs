namespace Framework.Configuration.BLL;

using System.Collections.Generic;
using Framework.Notification.DTO;

public interface IAttachmentContainer
{
    IList<NotificationAttachmentDTO> Attachments { get; set; }
}
