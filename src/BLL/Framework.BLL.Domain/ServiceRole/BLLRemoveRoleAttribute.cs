using Framework.BLL.Domain.Helpers;
using Framework.BLL.Domain.ServiceRole.Base;

namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
///  Атрибут удаления объекта через клиенсткий фасадный слой
/// </summary>
public class BLLRemoveRoleAttribute : BLLServiceRoleAttribute, ICountTypeContainer
{
    /// <inheritdoc />
    public BLLRemoveRoleAttribute()
    {
    }

    [Obsolete(ObsoleteMessageHelper.LegacyCtorMessage, true)]
    public BLLRemoveRoleAttribute(bool customRemove)
    {
    }

    /// <summary>
    /// Удаление единственного объекта и/или коллекции объектов
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
