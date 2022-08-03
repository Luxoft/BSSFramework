using System;

using Framework.Exceptions;

namespace Framework.Notification
{
    /// <summary>
    /// Сервис обработки исключений в рамках скоупа
    /// </summary>
    public interface IScopedExceptionService : IExceptionProcessor
    {
        /// <summary>
        /// Логирование в системе исключения
        /// </summary>
        /// <param name="exception">Исключение</param>
        void Save(Exception exception);
    }
}
