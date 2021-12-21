using System;

using Framework.Exceptions;

namespace Framework.Notification
{
    /// <summary>
    /// Сервис обработки исключений
    /// </summary>
    public interface IExceptionService : IExceptionProcessor
    {
        /// <summary>
        /// Логирование в системе исключения
        /// </summary>
        /// <param name="exception">Исключение</param>
        void Save(Exception exception);
    }
}
