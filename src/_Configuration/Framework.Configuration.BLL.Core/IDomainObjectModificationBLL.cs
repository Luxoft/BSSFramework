using Framework.Configuration.Domain;
using Framework.Core;

namespace Framework.Configuration.BLL;

public partial interface IDomainObjectModificationBLL
{
    /// <summary>
    /// Обработка модификаций хранимых в базе
    /// </summary>
    /// <param name="limit">Ограничение на количество обработанных модификаций</param>
    /// <returns>Количество обработанных модификаций</returns>
    Task<ITryResult<int>> ProcessAsync(int limit = 1000, CancellationToken ct = default);

    /// <summary>
    /// Получение состояния обработки очереди
    /// </summary>
    /// <returns></returns>
    QueueProcessingState GetProcessingState();
}

