using System;

using Framework.Exceptions;

namespace Framework.Notification
{
    /// <summary>
    /// Сервис обработки исключений в рамках скоупа
    /// </summary>
    public interface IExceptionExpander : IExceptionProcessor
    {
    }

    public interface IExceptionStorage
    {
        void Save(Exception exception);
    }
}
