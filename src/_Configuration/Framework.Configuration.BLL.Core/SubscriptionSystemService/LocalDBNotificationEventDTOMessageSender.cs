using System;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification.DTO;

using JetBrains.Annotations;

namespace Framework.Configuration.BLL
{
    /// <summary>
    /// Sender для отправки нотификакий в локальную бд
    /// </summary>
    public class LocalDBNotificationEventDTOMessageSender : BLLContextContainer<IConfigurationBLLContext>, IMessageSender<NotificationEventDTO>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст утилит</param>
        public LocalDBNotificationEventDTOMessageSender([NotNull] IConfigurationBLLContext context)
            : base(context)
        {
        }

        /// <inheritdoc />
        public void Send([NotNull] NotificationEventDTO dto)
        {
            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var serializedData = DataContractSerializerHelper.Serialize(dto);
            var dbNotification = new DomainObjectNotification
            {
                SerializeData = serializedData,
                Size = serializedData.Length
            };

            this.Context.Logics.DomainObjectNotification.Save(dbNotification);
        }
    }
}
