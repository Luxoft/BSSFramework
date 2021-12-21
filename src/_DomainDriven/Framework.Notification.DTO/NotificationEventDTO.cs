using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Framework.Notification.DTO
{
    [DataContract]
    public class NotificationEventDTO
    {
        [DataMember]
        public IList<NotificationTargetDTO> Targets;

        [DataMember]
        public IList<NotificationAttachmentDTO> Attachments;

        [DataMember]
        public string From;

        [DataMember]
        public string FromName;

        [DataMember]
        public string Subject;

        [DataMember]
        public NotificationMessage Message;


        [DataMember]
        public NotificationTechnicalInformationDTO TechnicalInformation;

        public NotificationEventDTO()
        {
        }

        public NotificationEventDTO(Notification notification)
        {
            var mailMessage = notification.Message;
            var technicalInformation = notification.TechnicalInformation;
            this.From = mailMessage.From.Address;
            this.FromName = mailMessage.From.DisplayName;

            this.Targets = mailMessage
                .To.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.To))
                .Concat(mailMessage.CC.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.Copy)))
                .Concat(mailMessage.ReplyToList.Select(z => Tuple.Create(z.Address, NotificationTargetTypes.ReplyTo)))
                .Select(z => new NotificationTargetDTO() { Name = z.Item1, Type = z.Item2 })
                .ToList();
            this.Subject = mailMessage.Subject;

            this.Message = new NotificationMessage()
                               {
                                   IsBodyHtml = mailMessage.IsBodyHtml,
                                   Message = mailMessage.Body
                               };

            this.Attachments =  mailMessage.Attachments.Select(z =>
            {
                byte[] content;
                using (var reader = new BinaryReader(z.ContentStream))
                {
                    content = reader.ReadBytes((int)z.ContentStream.Length);
                    //todo check size
                }
                return new NotificationAttachmentDTO
                {
                    Content = content,
                    Extension = z.ContentType.Name,
                    Name = z.Name,
                    ContentId = z.ContentId,
                    IsInline = z.ContentDisposition.Inline
                };
            }).ToList();

            this.TechnicalInformation = new NotificationTechnicalInformationDTO(technicalInformation);
        }
    }
}
