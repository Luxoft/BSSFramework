using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole.Base;

namespace Framework.BLL.Domain.ServiceRole;

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
