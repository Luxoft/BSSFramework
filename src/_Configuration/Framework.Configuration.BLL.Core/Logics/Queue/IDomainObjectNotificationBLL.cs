using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public partial interface IDomainObjectNotificationBLL
    {
        /// <summary>
        /// Получение состояния обработки очереди
        /// </summary>
        /// <returns></returns>
        QueueProcessingState GetProcessingState();
    }
}