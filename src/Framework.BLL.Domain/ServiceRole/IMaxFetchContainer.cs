using Framework.BLL.Domain.Dto;

namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Контейнер для маркировки размера выгрузки данных их базы
/// </summary>
public interface IMaxFetchContainer
{
    /// <summary>
    /// Максимальный уровень выгрузки из базы
    /// </summary>
    ViewDTOType MaxFetch { get; }
}
