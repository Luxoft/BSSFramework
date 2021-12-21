using Framework.Transfering;

namespace Framework.DomainDriven.BLL
{
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
}
