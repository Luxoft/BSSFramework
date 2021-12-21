using System;

namespace Framework.Exceptions
{
    /// <summary>
    /// Расширения для работы с сервисом исключений
    /// </summary>
    public static class ExceptionServiceExtensions
    {
        /// <summary>
        /// Раскрытие исключения
        /// </summary>
        /// <param name="exceptionProcessor">Сервис исключений</param>
        /// <param name="exception">Базовое исключение</param>
        /// <param name="handleSelf">Флаг перехвата исключения в процессе его раскрытия</param>
        /// <returns></returns>
        public static Exception Process(this IExceptionProcessor exceptionProcessor, Exception exception, bool handleSelf)
        {
            if (handleSelf)
            {
                try
                {
                    return exceptionProcessor.Process(exception);
                }
                catch (Exception ex)
                {
                    return ex;
                }
            }
            else
            {
                return exceptionProcessor.Process(exception);
            }
        }
    }
}
