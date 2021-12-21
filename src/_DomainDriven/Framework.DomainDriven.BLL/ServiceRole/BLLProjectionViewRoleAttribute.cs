using Framework.Transfering;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Атрибут указания генерации фасадного и BLL слоёв для проекций
    /// </summary>
    public class BLLProjectionViewRoleAttribute : BLLViewRoleBaseAttribute
    {
        /// <summary>
        /// Максимальный уровень выгрузки из базы
        /// </summary>
        public ViewDTOType MaxFetch { get; set; } = ViewDTOType.ProjectionDTO;

        /// <summary>
        /// Максимальный уровень выгрузки из базы
        /// </summary>
        protected override ViewDTOType BaseMaxFetch => this.MaxFetch;
    }
}