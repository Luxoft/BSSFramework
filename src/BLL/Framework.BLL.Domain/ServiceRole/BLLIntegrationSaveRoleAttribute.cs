using Framework.BLL.Domain.Helpers;
using Framework.BLL.Domain.ServiceRole.Base;

namespace Framework.BLL.Domain.ServiceRole;

/// <summary>
/// Атрибут для приёма изменений объекта через интеграцию (так же дополнительно управляет IntegrationSaveModelType-моделями)
/// </summary>
public class BLLIntegrationSaveRoleAttribute : BLLServiceRoleAttribute, IAllowCreateAttribute, ICountTypeContainer
{
    /// <inheritdoc />
    public BLLIntegrationSaveRoleAttribute()
    {
    }

    [Obsolete(ObsoleteMessageHelper.LegacyCtorMessage, true)]
    public BLLIntegrationSaveRoleAttribute(bool customIntegration)
    {
    }

    /// <inheritdoc />
    public bool AllowCreate { get; set; } = true;

    /// <summary>
    /// Сохранение единственного объекта и/или коллекции объектов
    /// </summary>
    public CountType CountType { get; set; } = CountType.Single;
}
