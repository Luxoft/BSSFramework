using Framework.Core;

namespace Framework.DomainDriven.BLL;

/// <summary>
/// Атрибут сохранения объекта через клиенсткий фасадный слой (так же дополнительно управляет CreateModelType/ChangeModelType/ExtendedModelType-моделями)
/// </summary>
public class BLLSaveRoleAttribute : BLLServiceRoleAttribute, IAllowCreateAttribute, ICountTypeContainer
{
    /// <inheritdoc />
    public BLLSaveRoleAttribute()
    {
    }

    [Obsolete(ObsoleteMessageHelper.LegacyCtorMessage, true)]
    public BLLSaveRoleAttribute(bool customSave)
    {
    }

    /// <inheritdoc />
    public bool AllowCreate { get; set; } = true;

    /// <summary>
    /// Метод сохранения объекта
    /// </summary>
    public BLLSaveType SaveType { get; set; } = BLLSaveType.Save;

    /// <summary>
    /// Сохранение единственного объекта и/или коллекции объектов
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
