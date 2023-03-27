using Framework.Core;

namespace Framework.DomainDriven.BLL;

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
