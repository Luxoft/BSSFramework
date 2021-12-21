using System;
using Framework.Transfering;

namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Базовый атрибут отображения объектов через фасадный слой (Visual, Simple, Full, Rich, Projection)
    /// </summary>
    public abstract class BLLViewRoleBaseAttribute : BLLServiceRoleAttribute, IMaxFetchContainer
    {
        /// <summary>
        /// Максимальный уровень выгрузки из базы
        /// </summary>
        protected abstract ViewDTOType BaseMaxFetch { get; }

        ViewDTOType IMaxFetchContainer.MaxFetch => this.BaseMaxFetch;
    }
}
